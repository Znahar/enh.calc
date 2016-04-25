using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Graphics.Drawables.Shapes;
//using Android

namespace EnhCalc.CustomShapes
{
    public class ChanceGraph : View
    {
        List<Range> chances;
        Paint failPaint;
        Paint successPaint;
        int padding = 10;
        int height = 100;
        StringBuilder log;
        public ChanceGraph(Context c, List<Range> ch, StringBuilder logBuffer) : base(c) {
            chances = ch ?? new List<Range>();
            log = logBuffer;
            failPaint = new Paint();
            failPaint.SetARGB(255, 255, 255, 255);
            failPaint.SetStyle(Paint.Style.Fill);
            successPaint = new Paint();
            successPaint.SetARGB(150, 0, 255, 0);
            successPaint.SetStyle(Paint.Style.Fill);
            //chances = new List<Range>() { new Range() { from = 0.25m, to = 0.50m} , new Range() { from=0.75m, to=1.20m } };
        }
        public void SetChances(List<Range> ranges) {
            chances = ranges;
        }
        protected override void OnDraw(Canvas canvas)
        {
            height = canvas.Height;
            var ss = new ShapeDrawable(new RectShape());
            ss.Paint.Set(successPaint);
            var fs = new ShapeDrawable(new RectShape());
            fs.Paint.Set(failPaint);
            int totalWiddth = canvas.Width - padding * 2;
            fs.SetBounds(padding, 0, padding + totalWiddth, height);
            fs.Draw(canvas);
            CalculateDrawable(chances).ForEach(r => {
                ss.SetBounds((int)(padding + totalWiddth * r.from), 0, (int)(padding + totalWiddth * r.to), height);
                ss.Draw(canvas);
            });
        }
        List<Range> CalculateDrawable(List<Range> chances)
        {
            var res = new List<Range>();
            chances.ForEach(r => {
                if (r.to < 1 && r.from < 1) res.Add(r);
                else if (r.from < 1 && r.to > 1)
                {
                    res.Add(new Range() { from = r.from, to = 1 });
                    res.Add(new Range() { from = 0, to = r.to - 1 });
                }
                else
                    res.Add(new Range() { from = r.from-1, to = r.to -1});
            });
            return res;
        }
    }
}