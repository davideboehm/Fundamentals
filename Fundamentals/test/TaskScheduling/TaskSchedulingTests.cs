using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Fundamentals.Timing.TaskScheduling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.TaskScheduling
{
    [TestClass]
    public class TaskSchedulingTests
    {
        [TestMethod]
        public void TestSimple()
        {
            List<int> testList = new List<int>();
            List<long> testList2 = new List<long>();
            var test = new TaskRunner<Task>();
            int i = 0;
            Stopwatch sw = Stopwatch.StartNew();
            for (int x = 0; x < 10; x++)
            {
                test.AddNewTask(new Task(
                    () =>
                    {
                        testList2.Add(sw.ElapsedMilliseconds);
                        testList.Add(i++);
                    }));
            }
            Thread.Sleep(1000);

            var averagePercentageOff = 0.0;
            var offset = testList2[0] - 100;
            for (int x = 0; x < 10; x++)
            {
                Assert.AreEqual(x, testList[x]);
                var expected = (x + 1.0) * 100.0;
                var actual = (testList2[x] - offset);
                averagePercentageOff += (actual - expected) / expected;
            }
            Assert.IsTrue(averagePercentageOff / 10.0 < 0.10);
        }
        

        [TestMethod]
        public void TestRateLimited()
        {
            List<int> testList = new List<int>();
            List<long> testList2 = new List<long>();
            var timestep = 200;
            var test = new RateLimitedTaskRunner<Task>(timestep);
            int i = 0;
            Stopwatch sw = Stopwatch.StartNew();
            for (int x = 0; x < 10; x++)
            {
                test.AddNewTask(new Task(
                    () =>
                    {
                        testList2.Add(sw.ElapsedMilliseconds);
                        testList.Add(i++);
                    }));
            }
            Thread.Sleep(timestep*15);

            var averagePercentageOff = 0.0;
            var offset = testList2[0]; 
            for (int x = 0; x < 10; x++)
            {
                Assert.AreEqual(x, testList[x]);
                var expected = x * timestep + offset;
                var actual = testList2[x];
                averagePercentageOff += (actual - expected) / expected;
            }
            Assert.IsTrue(averagePercentageOff / 10.0 < 0.10);
        }
        
        [TestMethod]
        public void TestSchedulingOrdering()
        {
            List<int> testList = new List<int>();
            List<long> testList2 = new List<long>();
            var test = new ScheduledTaskRunner();
            int i = 0;
            Stopwatch sw = Stopwatch.StartNew();
            var start = DateTime.UtcNow;
            var timestep = 50.0;
            for (int x = 0; x < 10; x++)
            {
                test.AddNewTask(new ScheduledTask(start.AddMilliseconds((x+1)* timestep),
                    () =>
                    {
                        testList2.Add(sw.ElapsedMilliseconds);
                        testList.Add(i++);
                    }));
            }
            Thread.Sleep(1500);
            Assert.IsTrue(testList2.Count == 10);

            var averageAmountOff = 0.0;
            for (int x = 0; x < 10; x++)
            {
                Assert.AreEqual(x, testList[x]);
                var expected = (x + 1.0) * timestep;
                averageAmountOff += (testList2[x] - expected);
            }
            Assert.IsTrue(averageAmountOff / 10.0 < 10);
        }
        

        [TestMethod]
        public void TestRateLimitedSequential()
        {
            List<int> testList = new List<int>();
            List<long> testList2 = new List<long>();
            var timestep = 200;
            var test = new RateLimitedSequentialTaskRunner<Task>(timestep);
            int i = 0;
            Stopwatch sw = Stopwatch.StartNew();
            var offset = 0.0;


            test.AddNewTask(new Task(
                    () =>
                    {
                        Thread.Sleep(250);
                        offset = sw.ElapsedMilliseconds;
                    }));
            for (int x = 0; x < 10; x++)
            {
                test.AddNewTask(new Task(
                    () =>
                    {
                        testList2.Add(sw.ElapsedMilliseconds);
                        testList.Add(i++);
                    }));
            }
            Thread.Sleep(timestep *15);
            var averagePercentageOff = 0.0;
            for (int x = 0; x < 10; x++)
            {
                Assert.AreEqual(x, testList[x]);
                var expected = (x * timestep) + offset;
                var actual = testList2[x];
                averagePercentageOff += (actual - expected) / expected;
            }
            Assert.IsTrue(averagePercentageOff / 10.0 < 0.10);
        }
    }
}
