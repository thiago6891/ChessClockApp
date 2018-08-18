using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessClockApp.Tests
{
    [TestClass]
    public class ChessClockSpecs
    {
        private readonly TimeSpan _errorMargin = TimeSpan.FromMilliseconds(20);

        private bool IsWithinErrorMargin(TimeSpan expected, TimeSpan actual) =>
            expected - _errorMargin <= actual && actual <= expected + _errorMargin;

        private TimeSpan GetRandomTimeMinutes(int minTime, int maxTime) =>
            TimeSpan.FromMinutes((new Random()).Next(minTime, maxTime));

        private TimeSpan GetRandomTimeSeconds(int minTime, int maxTime) =>
            TimeSpan.FromSeconds((new Random()).Next(minTime, maxTime));

        private TimeSpan GetRandomTimeMilliseconds(int minTime, int maxTime) =>
            TimeSpan.FromMilliseconds((new Random()).Next(minTime, maxTime));

        [TestMethod]
        public void EitherPlayerCanStartTheClock()
        {
            var gameTime = GetRandomTimeMinutes(1, 30);
            var clock1 = new NoDelayChessClock(gameTime);
            var clock2 = new NoDelayChessClock(gameTime);
            var waitTime = GetRandomTimeMilliseconds(100, 200);

            clock1.PressButton(Player.ONE);
            clock2.PressButton(Player.TWO);
            Thread.Sleep(waitTime);
            var playerOneTimeLeft = clock2.GetRemainingTime(Player.ONE);
            var playerTwoTimeLeft = clock1.GetRemainingTime(Player.TWO);

            Assert.AreEqual(gameTime, clock1.GetRemainingTime(Player.ONE));
            Assert.IsTrue(IsWithinErrorMargin(gameTime - waitTime, playerTwoTimeLeft));
            Assert.AreEqual(gameTime, clock2.GetRemainingTime(Player.TWO));
            Assert.IsTrue(IsWithinErrorMargin(gameTime - waitTime, playerOneTimeLeft));
        }

        [TestMethod]
        public void PressingTheSameButtonTwiceInARowHasNoEffect()
        {
            var gameTime = GetRandomTimeMinutes(1, 30);
            var clock = new NoDelayChessClock(gameTime);
            var waitTime = GetRandomTimeMilliseconds(100, 200);

            clock.PressButton(Player.TWO);
            Thread.Sleep(waitTime);
            clock.PressButton(Player.TWO);
            Thread.Sleep(waitTime);
            var playerOneTimeLeft = clock.GetRemainingTime(Player.ONE);

            Assert.AreEqual(gameTime, clock.GetRemainingTime(Player.TWO));
            Assert.IsTrue(IsWithinErrorMargin(gameTime - waitTime - waitTime, playerOneTimeLeft));
        }

        [TestMethod]
        public void ClockShouldCountdownAsExpected()
        {
            var gameTime = GetRandomTimeMinutes(1, 30);
            var clock = new NoDelayChessClock(gameTime);
            var playerOneWaitTime = GetRandomTimeMilliseconds(100, 200);
            var playerTwoWaitTime = GetRandomTimeMilliseconds(100, 200);

            clock.PressButton(Player.TWO);
            Thread.Sleep(playerOneWaitTime);
            clock.PressButton(Player.ONE);
            var playerOneTimeLeft = clock.GetRemainingTime(Player.ONE);
            Thread.Sleep(playerTwoWaitTime);
            clock.PressButton(Player.TWO);
            var playerTwoTimeLeft = clock.GetRemainingTime(Player.TWO);

            Assert.IsTrue(IsWithinErrorMargin(gameTime - playerOneWaitTime, playerOneTimeLeft));
            Assert.IsTrue(IsWithinErrorMargin(gameTime - playerTwoWaitTime, playerTwoTimeLeft));
        }

        [TestMethod]
        // Under FIDE and US Chess rules, the Fischer increment should be applied on the first move as well.
        public void FischerIncrementsShouldBeAppliedOnEveryMove()
        {
            var gameTime = GetRandomTimeMinutes(1, 30);
            var incrementTime = GetRandomTimeSeconds(1, 10);
            var clock = new FischerDelayChessClock(gameTime, incrementTime);
            var waitTime = GetRandomTimeMilliseconds(100, 200);

            var playerTwoTimeLeft = clock.GetRemainingTime(Player.TWO);
            clock.PressButton(Player.TWO);
            Thread.Sleep(waitTime);
            clock.PressButton(Player.ONE);
            var playerOneTimeLeft = clock.GetRemainingTime(Player.ONE);

            Assert.IsTrue(IsWithinErrorMargin(gameTime - waitTime + incrementTime, playerOneTimeLeft));
            Assert.AreEqual(gameTime, playerTwoTimeLeft);
        }

        [TestMethod]
        public void BronsteinDelayShouldBeApplied()
        {
            var gameTime = GetRandomTimeMinutes(1, 30);
            var delay = GetRandomTimeMilliseconds(150, 200);
            var clock = new BronsteinDelayChessClock(gameTime, delay);
            var playerOneWaitTime = delay - GetRandomTimeMilliseconds(50, 100);
            var playerTwoWaitTime = delay + GetRandomTimeMilliseconds(50, 100);

            clock.PressButton(Player.TWO);
            Thread.Sleep(playerOneWaitTime);
            clock.PressButton(Player.ONE);
            var playerOneTimeLeft = clock.GetRemainingTime(Player.ONE);
            Thread.Sleep(playerTwoWaitTime);
            clock.PressButton(Player.TWO);
            var playerTwoTimeLeft = clock.GetRemainingTime(Player.TWO);

            Assert.IsTrue(IsWithinErrorMargin(gameTime + delay, playerOneTimeLeft));
            Assert.IsTrue(IsWithinErrorMargin(gameTime + delay - playerTwoWaitTime + delay, playerTwoTimeLeft));
        }

        [TestMethod]
        public void NormalDelayShouldBeApplied()
        {
            var gameTime = GetRandomTimeMinutes(1, 30);
            var delay = GetRandomTimeMilliseconds(150, 200);
            var clock = new NormalDelayChessClock(gameTime, delay);
            var playerOneWaitTime = delay - GetRandomTimeMilliseconds(50, 100);
            var playerTwoWaitTime = delay + GetRandomTimeMilliseconds(50, 100);

            clock.PressButton(Player.TWO);
            Thread.Sleep(playerOneWaitTime);
            clock.PressButton(Player.ONE);
            var playerOneTimeLeft = clock.GetRemainingTime(Player.ONE);
            Thread.Sleep(playerTwoWaitTime);
            clock.PressButton(Player.TWO);
            var playerTwoTimeLeft = clock.GetRemainingTime(Player.TWO);

            Assert.IsTrue(IsWithinErrorMargin(gameTime, playerOneTimeLeft));
            Assert.IsTrue(IsWithinErrorMargin(gameTime + delay - playerTwoWaitTime, playerTwoTimeLeft));
        }

        [TestMethod]
        public void BothTimersShouldFreezeWhenOneReachesZero()
        {
            var gameTime = GetRandomTimeMilliseconds(100, 200);
            var clock = new NoDelayChessClock(gameTime);
            var waitTime = GetRandomTimeMilliseconds(40, 60);

            clock.PressButton(Player.TWO);
            Thread.Sleep(waitTime);
            clock.PressButton(Player.ONE);
            Thread.Sleep(gameTime + _errorMargin);

            Assert.IsTrue(IsWithinErrorMargin(gameTime - waitTime, clock.GetRemainingTime(Player.ONE)));
            Assert.AreEqual(TimeSpan.Zero, clock.GetRemainingTime(Player.TWO));
        }

        [TestMethod]
        public void ClockCanBeReset()
        {
            var gameTime = GetRandomTimeMinutes(1, 30);
            var clock = new NoDelayChessClock(gameTime);
            var waitTime = GetRandomTimeMilliseconds(100, 200);

            clock.PressButton(Player.TWO);
            Thread.Sleep(waitTime);
            clock.PressButton(Player.ONE);
            Thread.Sleep(waitTime);
            clock.Reset();
            Thread.Sleep(waitTime);

            Assert.AreEqual(gameTime, clock.GetRemainingTime(Player.ONE));
            Assert.AreEqual(gameTime, clock.GetRemainingTime(Player.TWO));
        }
    }
}