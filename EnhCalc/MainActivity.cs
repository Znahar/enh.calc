using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using EnhCalc.CustomShapes;

using System.Text;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics.Drawables;

namespace EnhCalc
{
    [Activity(Label = "EnhCalc", MainLauncher = true, Icon = "@drawable/BDO_icon")]
    public class MainActivity : Activity
    {
        int count = 0;
        StringBuilder _logBuffer = new StringBuilder();
        TextView _logContainer;
        void Log(string s) { _logBuffer.AppendLine(s);
            if (_logContainer != null)
            {
                _logContainer.Text = _logBuffer.ToString();// + "1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12\n13\n14\n15\n16\n17\n18\n19\n20\n21\n22\n23\n24\n25\n26\n27\n28\n29\n31\n32\n33\n34\n35\n36\n37\n38\n39\n40\n41\n42\n43\n44\n45\n46\n47\n48\n49\n50\n51\n52\n53\n54\n55\n56\n57\n58\n59\n60\n61\n62\n63\n64\n65\n66\n67\n68\n69\n70\n71\n72\n73\n74\n75\n76\n77\n78\n79\n80\n81\n82\n83\n84\n85\n86\n87\n88\n89\n90\n91\n92\n93\n94\n95\n96\n97\n98\n99\n100\n1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12\n13\n14\n15\n16\n17\n18\n19\n20\n21\n22\n23\n24\n25\n26\n27\n28\n29\n31\n32\n33\n34\n35\n36\n37\n38\n39\n40\n41\n42\n43\n44\n45\n46\n47\n48\n49\n50\n51\n52\n53\n54\n55\n56\n57\n58\n59\n60\n61\n62\n63\n64\n65\n66\n67\n68\n69\n70\n71\n72\n73\n74\n75\n76\n77\n78\n79\n80\n81\n82\n83\n84\n85\n86\n87\n88\n89\n90\n91\n92\n93\n94\n95\n96\n97\n98\n99\n200\n1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12\n13\n14\n15\n16\n17\n18\n19\n20\n21\n22\n23\n24\n25\n26\n27\n28\n29\n31\n32\n33\n34\n35\n36\n37\n38\n39\n40\n41\n42\n43\n44\n45\n46\n47\n48\n49\n50\n51\n52\n53\n54\n55\n56\n57\n58\n59\n60\n61\n62\n63\n64\n65\n66\n67\n68\n69\n70\n71\n72\n73\n74\n75\n76\n77\n78\n79\n80\n81\n82\n83\n84\n85\n86\n87\n88\n89\n90\n91\n92\n93\n94\n95\n96\n97\n98\n99\n300";
            }
        }

        EnhancementSession CurrentSession;

        protected override void OnCreate(Bundle bundle)
        {
            //start
            //start!!
            base.OnCreate(bundle);
            ActionBar.Hide();
            Log("Start...");
            try {
                ShowMain(this, null);
            }
            catch ( Exception e)
            {
                Log(e.ToString());
                ShowLog(this, null);
            }
        }

        void ShowMain(object o, EventArgs a)
        {
            SetContentView(Resource.Layout.Main);
            _logContainer = FindViewById<TextView>(Resource.Id.LogText);
            var dm = GetDisplayMetrics();
            var itemTypeButton = FindViewById<Button>(Resource.Id.ItemType);
            FindViewById<Button>(Resource.Id.SwitchLayout).Click += ShowLog;
            itemTypeButton.Click += ItemTypePopup;
            itemTypeButton.SetWidth(dm.WidthPixels / 2);
            var itemLevelButton = FindViewById<Button>(Resource.Id.ItemLevel);
            itemLevelButton.SetWidth(dm.WidthPixels / 2);
            itemLevelButton.Enabled = false;
            itemLevelButton.Click += ItemLevelPopup;
            var failEnhButton = FindViewById<Button>(Resource.Id.FailEnhancement);
            failEnhButton.Enabled = false;
            failEnhButton.Click += FailButtonClick;
            FindViewById<LinearLayout>(Resource.Id.historyScroll).LayoutParameters.Height = ((int)(dm.HeightPixels * 0.4));
            FindViewById<LinearLayout>(Resource.Id.chanceGraph).LayoutParameters.Height = ((int)(dm.HeightPixels * 0.2));
            var cg = FindViewById<LinearLayout>(Resource.Id.chanceGraph);
            cg.AddView(new ChanceGraph(ApplicationContext, CurrentSession?.CurrentPositions, _logBuffer));
            if(CurrentSession != null)
                FillHistory();
            //var scroll = (ScrollView)FindViewById<LinearLayout>(Resource.Id.historyScroll).GetChildAt(0);
            //scroll.on
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.OptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            CurrentSession = null;
            ShowMain(this, null);
            return base.OnOptionsItemSelected(item);
        }
        private DisplayMetrics GetDisplayMetrics() {
            DisplayMetrics dm = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(dm);
            Log("Screen width: "+dm.WidthPixels.ToString());
            return dm;
        }
        private void ItemTypePopup(Object obj, EventArgs args)
        {
            var items = new List<ItemType>(EnhancementSession.chances.Keys);
            var menu = PrepareMenu(items.Select(i => i.ToString()).OrderBy(i => i),
                (o, a) => {
                    FindViewById<Button>(Resource.Id.ItemType).Text = a.Item.TitleFormatted.ToString();
                    Log(a.Item.TitleFormatted.ToString() + " selected");
                    var il = FindViewById<Button>(Resource.Id.ItemLevel);
                    il.Enabled = true;
                    il.Text = "ENH LEVEL";
                    FindViewById<Button>(Resource.Id.FailEnhancement).Enabled = false;
                },
                FindViewById<Button>(Resource.Id.ItemType)
            );
            menu.Inflate(Resource.Menu.ItemTypeMenu);
            menu.Show();
        }
        private void ItemLevelPopup(Object obj, EventArgs args)
        {
            try
            {
                var key = (ItemType)Enum.Parse(typeof(ItemType), FindViewById<Button>(Resource.Id.ItemType).Text);
                var items = new List<uint>(EnhancementSession.chances[key].Keys);
                var menu = PrepareMenu(items.Select(i => i.ToString()).Concat(new[] { "0" }).OrderBy(i => i),
                    (o, a) => {
                        FindViewById<Button>(Resource.Id.ItemLevel).Text = a.Item.TitleFormatted.ToString();
                        Log(a.Item.TitleFormatted.ToString() + " selected");
                        var feb = FindViewById<Button>(Resource.Id.FailEnhancement);
                        feb.Enabled = CurrentSession == null ? true : CurrentSession.CheckFailAvailableFor(new Item() { Type = key, enhLevel = uint.Parse(a.Item.TitleFormatted.ToString())});
                        feb.Text = "FAIL ENHANCEMENT (success rate "+EnhancementSession.chances[key][uint.Parse(a.Item.TitleFormatted.ToString())+1]+")";
                    },
                    FindViewById<Button>(Resource.Id.ItemLevel));
                menu.Menu.GetItem(menu.Menu.Size() - 1).SetEnabled(false);
                menu.Inflate(Resource.Menu.ItemTypeMenu);
                menu.Show();
            }
            catch (Exception e) { Log(e.ToString()); ShowLog(this, null); }
        }
        private void FailButtonClick(Object obj, EventArgs args)
        {
            var item = new Item();
            item.Type = ToItemType(FindViewById<Button>(Resource.Id.ItemType).Text);
            item.enhLevel = uint.Parse(FindViewById<Button>(Resource.Id.ItemLevel).Text);
            Log(string.Format("Failing Enhancement for item {0} of level {1}", item.Type, item.enhLevel));
            if (CurrentSession == null)
                CurrentSession = EnhancementSession.Start(item);
            else
                CurrentSession.FailEnhance(item);
            FillHistory();
            var cg = FindViewById<LinearLayout>(Resource.Id.chanceGraph);
            cg.RemoveAllViews();
            cg.AddView(new ChanceGraph(ApplicationContext, CurrentSession.CurrentPositions, _logBuffer));
            FindViewById<Button>(Resource.Id.FailEnhancement).Enabled = CurrentSession.CheckFailAvailableFor(item);
        }
        void ShowLog(object o, EventArgs a)
        {
            SetContentView(Resource.Layout.LogLayout);
            _logContainer = FindViewById<TextView>(Resource.Id.LogText);
            FindViewById<Button>(Resource.Id.SwitchLayout).Click += ShowMain;
            FindViewById<Button>(Resource.Id.Flush).Click += (obj, args) => { _logBuffer.Clear(); Log("Log start...");  };
            FindViewById<TextView>(Resource.Id.LogText).Text = _logBuffer.ToString();
        }
        private PopupMenu PrepareMenu(IEnumerable<string> vals, EventHandler<PopupMenu.MenuItemClickEventArgs> itemClick, View anchor)
        {
            var m = new PopupMenu(this, anchor);
            foreach (var key in vals)
                m.Menu.Add(key.ToString());
            m.MenuItemClick += itemClick;
            return m;
        }

        private static ItemType ToItemType(string name) { return (ItemType)Enum.Parse(typeof(ItemType), name); }

        private void FillHistory()
        {
            var sb = new StringBuilder("History:\n");
            FindViewById<TextView>(Resource.Id.EnhancementHistory).Text =
                string.Join("\n",
                    CurrentSession.History.Select((hi, i) => (string.Format("Attempt {4:D2}. Item:{2} lvl {3}. Basic chance: {5}\n Values Before: {0}\nValues after: {1}\n",
                        string.Join(",", hi.PositionsBefore.Select(pb => pb.ToString())),
                        string.Join(",", hi.PositionsAfter.Select(pb => pb.ToString())),
                        hi.Item.Type,
                        hi.Item.enhLevel,
                        i,
                        EnhancementSession.chances[hi.Item.Type][hi.Item.enhLevel + 1]
                    ))).OrderByDescending(s => s)
                );
        }
    }
}

