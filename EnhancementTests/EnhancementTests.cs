using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnhCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhCalc.Tests
{
    [TestClass()]
    public class EnhancementTests
    {
        [TestMethod()]
        public void StartTest()
        {
            var s = EnhancementSession.Start(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            Assert.AreEqual(s.CurrentPositions.Last(), new Range() { from = 0.5m, to = 1.25m });
        }

        [TestMethod()]
        public void FailEnhanceTest1()
        {
            var s = EnhancementSession.Start(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            Assert.AreEqual(s.CurrentPositions.Last(), new Range() { from = 0.75m, to = 1.25m });
        }
        [TestMethod()]
        public void FailEnhanceTestProcessLoweringRange()
        {
            EnhancementSession.chances.Add(ItemType.Another, new Dictionary<uint, decimal>() { { 1, 0.1m }, { 2, 0.05m} });
            var s = EnhancementSession.Start(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 1 });
            Assert.AreEqual(2, s.CurrentPositions.Count);
            Assert.AreEqual(new Range() { from = 0.82m, to = 1.07m }, s.CurrentPositions[0]);
            Assert.AreEqual(new Range() { from = 0.14m, to = 0.32m }, s.CurrentPositions[1]);
            s.FailEnhance(new Item() { Type = ItemType.Another, enhLevel = 1 });
            Assert.AreEqual(3, s.CurrentPositions.Count);
            Assert.AreEqual(new Range() { from = 0.87m, to = 1.05m }, s.CurrentPositions[0]);
            Assert.AreEqual(new Range() { from = 0.10m, to = 0.12m }, s.CurrentPositions[1]);
            Assert.AreEqual(new Range() { from = 0.19m, to = 0.37m }, s.CurrentPositions[2]);
        }
        [TestMethod()]
        public void FailEnhanceTestProcessLowRaise()
        {
            var s = EnhancementSession.Start(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 1 });
            s.FailEnhance(new Item() { Type = ItemType.Another, enhLevel = 1 });
            s.FailEnhance(new Item() { Type = ItemType.Another, enhLevel = 0 });
            Assert.AreEqual(3, s.CurrentPositions.Count);
            Assert.AreEqual(new Range() { from = 0.97m, to = 1.1m }, s.CurrentPositions[0]);
            Assert.AreEqual(new Range() { from = 0.20m, to = 0.22m }, s.CurrentPositions[1]);
            Assert.AreEqual(new Range() { from = 0.29m, to = 0.47m }, s.CurrentPositions[2]);
        }
        [TestMethod()]
        public void FailEnhanceTestProcessRaise()
        {
            var s = EnhancementSession.Start(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 1 });
            s.FailEnhance(new Item() { Type = ItemType.Another, enhLevel = 1 });
            s.FailEnhance(new Item() { Type = ItemType.Another, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            Assert.AreEqual(2, s.CurrentPositions.Count);
            Assert.AreEqual(new Range() { from = 0.22m, to = 0.25m }, s.CurrentPositions[0]);
            Assert.AreEqual(new Range() { from = 0.54m, to = 0.72m }, s.CurrentPositions[1]);
        }
        [TestMethod()]
        public void FailEnhanceTestProcessOver100()
        {
            var s = EnhancementSession.Start(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 0 });
            s.FailEnhance(new Item() { Type = ItemType.Jewelery, enhLevel = 1 });
            Assert.AreEqual(1, s.CurrentPositions.Count);
            Assert.AreEqual(new Range() { from = 0.14m, to = 0.32m }, s.CurrentPositions[0]);
        }
    }
}