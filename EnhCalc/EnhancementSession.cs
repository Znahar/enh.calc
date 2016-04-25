using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EnhCalc
{
    public struct Item {
        public ItemType Type;
        public uint enhLevel;
    }
    public enum ItemType {
        Jewelery,
        Armor,
        Weapon,
        Costume,
        Another
    }
    public class HistoryRecord
    {
        public Item Item;
        public List<Range> PositionsBefore;
        public List<Range> PositionsAfter;
    }
    public struct Range
    {
        public decimal from;
        public decimal to;

        public override string ToString()
        {
            return string.Format("from={0}, to={1}",from, to);
        }
    }
    public class EnhancementSession
    {
        public static readonly Dictionary<ItemType, Dictionary<uint, decimal>> chances = new Dictionary<ItemType, Dictionary<uint, decimal>>() {
            {ItemType.Jewelery, new Dictionary<uint, decimal>() { { 1, 0.25m },
                                                                 { 2, 0.07m } } },

            //{ItemType.Costume, new Dictionary<uint, decimal>() {  { 1, 0.07m }, { 2, 0.05m} } },
            //{ItemType.Another, new Dictionary<uint, decimal>() {  { 1, 0.1m } } },

            {ItemType.Weapon, new Dictionary<uint, decimal>()  { { 1, 1 },
                                                                { 2, 1 },
                                                                { 3, 1 },
                                                                { 4, 1 },
                                                                { 5, 1 },
                                                                { 6, 1 },
                                                                { 7, 1 },
                                                                { 8, 0.2m },
                                                                { 9, 0.175m },
                                                                { 10, 0.15m },
                                                                { 11, 0.125m },
                                                                { 12, 0.1m },
                                                                { 13, 0.075m },
                                                                { 14, 0.05m },
                                                                { 15, 0.025m },
                                                                { 16, 0.15m },
                                                                { 17, 0.075m },
                                                                { 18, 0.05m },
                                                                { 19, 0.02m },
                                                                { 20, 0.015m },
            }  },
            /*
            { ItemType.Armor, new Dictionary<uint, decimal>()  { { 1, 1 },
                                                                { 2, 1 },
                                                                { 3, 1 },
                                                                { 4, 1 },
                                                                { 5, 1 },
                                                                { 6, 1 },
                                                                { 7, 1 },
                                                                { 8, 0.2m },
                                                                { 9, 0.2m },
                                                                { 10, 0.2m },
                                                                { 11, 0.2m },
                                                                { 12, 0.2m },
                                                                { 13, 0.2m },
                                                                { 14, 0.2m },
                                                                { 15, 0.2m },
            }},//*/
        };
        public List<HistoryRecord> History = new List<HistoryRecord>();
        public List<Range> CurrentPositions { get
            {
                if (History.Any())
                    return History.Last().PositionsAfter;
                return null;
            }
        }
        public static EnhancementSession Start(Item item)
        {
            var s = new EnhancementSession();
            var r = new HistoryRecord() {
                Item = item,
                PositionsAfter = new List<Range>() { new Range() {from = chances[item.Type][item.enhLevel+1]*2,
                                                                to   = chances[item.Type][item.enhLevel+1]+1}},
                PositionsBefore = new List<Range>() { new Range() { from = 0, to = 1} }
            };
            s.History.Add(r);
            return s;
        }
        public void FailEnhance(Item item)
        {
            var r = new HistoryRecord() {
                Item = item,
                PositionsBefore = CurrentPositions,
            };
            var enhChance = chances[item.Type][item.enhLevel + 1];
            r.PositionsAfter = CurrentPositions.SelectMany(p => CalculatePosition(p, enhChance)).ToList();
            NormalizeRecord(ref r);
            History.Add(r);

        }

        private void NormalizeRecord(ref HistoryRecord r)
        {
            //var l = r.PositionsAfter.Select(p => p.from < 1).ToList();
            if (r.PositionsAfter.Any(p => p.from >= 1 && p.to >= 1))
                for (int i = 0; i < r.PositionsAfter.Count; i++)
                    if(r.PositionsAfter[i].from >= 1 && r.PositionsAfter[i].to >= 1)
                        r.PositionsAfter[i] = new Range() { from = r.PositionsAfter[i].from-1, to = r.PositionsAfter[i].to -1};
        }
        private IEnumerable<Range> CalculatePosition(Range r, decimal enhChance)
        {
            var res = new List<Range>();
            var newLow = r.from + enhChance;
            //pos 10x-110, enhancement by 10 - cannot fail, range excluded
            if (r.to <= enhChance || (r.to > 1 && r.from > 1 && r.to - 1 <= enhChance)) return res;
            //pos 0-107, enhancement by 7 or greater
            else if (r.to > 1 && r.to - 1 <= enhChance) res.Add(new Range() { from = newLow, to = 1 + enhChance });
            //pos 0-125, enhancement by 7 results in 2 ranges: 7-107, 114-132
            else if(( r.from < enhChance && r.to > enhChance || r.to > 1 && r.to -1 > enhChance))
            {
                if (r.to > 1)
                {
                    res.Add(new Range() { from = newLow, to = 1 + enhChance });
                    res.Add(new Range() { from = 1 + enhChance * 2, to = r.to + enhChance });
                }
                else
                    res.Add(new Range() { from = enhChance * 2, to = r.to + enhChance });
            }
            else { res.Add(new Range() { from = newLow, to = r.to + enhChance}); }
            return res;
        }
        public bool CheckFailAvailableFor(Item item)
        {
            return CurrentPositions.Any(p => !(p.from >= 0 && p.to <= chances[item.Type][item.enhLevel+1]));
        }
    }
}