using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace ChessClockApp.Tests
{
    [TestClass]
    public class ChessClockSpecs
    {
        private readonly TimeSpan _errorMargin = TimeSpan.FromMilliseconds(10);

        private bool IsWithinErrorMargin(TimeSpan expected, TimeSpan actual) =>
            expected - _errorMargin <= actual && actual <= expected + _errorMargin;

        [TestMethod]
        public void ChessClockShouldConfigureBothPlayersTime()
        {
            var expectedWhiteTime = TimeSpan.FromMinutes(5);
            var expectedBlackTime = TimeSpan.FromMinutes(4);
            var clock = new ChessClock(expectedWhiteTime, expectedBlackTime);

            var actualWhiteTime = clock.WhiteRemainingTime;
            var actualBlackTime = clock.BlackRemainingTime;

            Assert.AreEqual(expectedWhiteTime, actualWhiteTime);
            Assert.AreEqual(expectedBlackTime, actualBlackTime);
        }

        [TestMethod]
        public void ChessClockShouldDecrementBothPlayersTime()
        {
            var whiteTime = TimeSpan.FromMinutes(5);
            var blackTime = TimeSpan.FromMinutes(4);
            var clock = new ChessClock(whiteTime, blackTime);
            var expectedWhiteWaitTime = TimeSpan.FromMilliseconds(500);
            var expectedBlackWaitTime = TimeSpan.FromMilliseconds(400);

            clock.PressBlackStopButton();
            Thread.Sleep(expectedWhiteWaitTime);
            clock.PressWhiteStopButton();
            var actualWhiteWaitTime = whiteTime - clock.WhiteRemainingTime;
            Thread.Sleep(expectedBlackWaitTime);
            clock.PressBlackStopButton();
            var actualBlackWaitTime = blackTime - clock.BlackRemainingTime;

            Assert.IsTrue(IsWithinErrorMargin(expectedWhiteWaitTime, actualWhiteWaitTime));
            Assert.IsTrue(IsWithinErrorMargin(expectedBlackWaitTime, actualBlackWaitTime));
        }

        [TestMethod]
        public void ChessClockShouldApplyIncrementWhenPlayerFinishesTurn()
        {
            var whiteTime = TimeSpan.FromMinutes(5);
            var blackTime = TimeSpan.FromMinutes(4);
            var whiteIncrement = TimeSpan.FromSeconds(1);
            var blackIncrement = TimeSpan.FromSeconds(2);
            var clock = new ChessClock(whiteTime, blackTime, whiteIncrement, blackIncrement);

            clock.PressBlackStopButton();
            clock.PressWhiteStopButton();
            var actualWhiteTime = clock.WhiteRemainingTime;
            clock.PressBlackStopButton();
            var actualBlackTime = clock.BlackRemainingTime;

            var expectedWhiteTime = whiteTime + whiteIncrement;
            var expectedBlackTime = blackTime + blackIncrement;
            Assert.IsTrue(IsWithinErrorMargin(expectedWhiteTime, actualWhiteTime));
            Assert.IsTrue(IsWithinErrorMargin(expectedBlackTime, actualBlackTime));
        }

        [TestMethod]
        public void TheFirstTimeTheBlackButtonIsPressedShouldNotIncrementTheTime()
        {
            var whiteTime = TimeSpan.FromMinutes(5);
            var blackTime = TimeSpan.FromMinutes(4);
            var whiteIncrement = TimeSpan.FromSeconds(1);
            var blackIncrement = TimeSpan.FromSeconds(2);
            var clock = new ChessClock(whiteTime, blackTime, whiteIncrement, blackIncrement);

            clock.PressBlackStopButton();
            var actualBlackTime = clock.BlackRemainingTime;

            Assert.AreEqual(blackTime, actualBlackTime);
        }

        [TestMethod]
        public void PressingTheSameStopButtonTwiceInARowHasNoEffect()
        {
            var whiteTime = TimeSpan.FromMinutes(5);
            var blackTime = TimeSpan.FromMinutes(4);
            var clock = new ChessClock(whiteTime, blackTime);
            var whiteWaitTime = TimeSpan.FromMilliseconds(250);
            var blackWaitTime = TimeSpan.FromMilliseconds(200);
            var expectedWhiteRemainingTime = whiteTime - (whiteWaitTime + whiteWaitTime);
            var expectedBlackRemainingTime = blackTime - (blackWaitTime + blackWaitTime);

            clock.PressBlackStopButton();
            Thread.Sleep(whiteWaitTime);
            clock.PressBlackStopButton();
            Thread.Sleep(whiteWaitTime);
            clock.PressWhiteStopButton();
            var actualWhiteRemainingTime = clock.WhiteRemainingTime;
            Thread.Sleep(blackWaitTime);
            clock.PressWhiteStopButton();
            Thread.Sleep(blackWaitTime);
            clock.PressBlackStopButton();
            var actualBlackRemainingTime = clock.BlackRemainingTime;

            Assert.IsTrue(IsWithinErrorMargin(expectedWhiteRemainingTime, actualWhiteRemainingTime));
            Assert.IsTrue(IsWithinErrorMargin(expectedBlackRemainingTime, actualBlackRemainingTime));
        }

        [TestMethod]
        public void PressingTheBlackButtonStartsTheGame()
        {
            var whiteTime = TimeSpan.FromMinutes(5);
            var blackTime = TimeSpan.FromMinutes(4);
            var clock = new ChessClock(whiteTime, blackTime);

            clock.PressBlackStopButton();
            Thread.Sleep(100);

            Assert.IsTrue(clock.WhiteRemainingTime < whiteTime);
        }

        [TestMethod]
        public void PressingTheWhiteButtonDoesNotStartTheGame()
        {
            var whiteTime = TimeSpan.FromMinutes(5);
            var blackTime = TimeSpan.FromMinutes(4);
            var clock = new ChessClock(whiteTime, blackTime);

            clock.PressWhiteStopButton();
            Thread.Sleep(100);

            Assert.AreEqual(whiteTime, clock.WhiteRemainingTime);
        }

        [TestMethod]
        public void ChessClockCanBeStopped()
        {
            var whiteTime = TimeSpan.FromMinutes(5);
            var blackTime = TimeSpan.FromMinutes(4);
            var clock = new ChessClock(whiteTime, blackTime);

            clock.PressBlackStopButton();
            Thread.Sleep(100);
            clock.Stop();
            var expectedWhiteTime = clock.WhiteRemainingTime;
            var expectedBlackTime = clock.BlackRemainingTime;
            Thread.Sleep(100);
            var actualWhiteTime = clock.WhiteRemainingTime;
            var actualBlackTime = clock.BlackRemainingTime;

            Assert.AreEqual(expectedWhiteTime, actualWhiteTime);
            Assert.AreEqual(expectedBlackTime, actualBlackTime);
        }

        [TestMethod]
        public void ChessClockCanNotBeResumedAfterStopping()
        {
            var whiteTime = TimeSpan.FromMinutes(5);
            var blackTime = TimeSpan.FromMinutes(4);
            var clock = new ChessClock(whiteTime, blackTime);

            clock.PressBlackStopButton();
            Thread.Sleep(100);
            clock.Stop();
            var expectedWhiteTime = clock.WhiteRemainingTime;
            var expectedBlackTime = clock.BlackRemainingTime;
            Thread.Sleep(100);
            clock.PressBlackStopButton();
            Thread.Sleep(100);
            clock.PressWhiteStopButton();
            Thread.Sleep(100);
            var actualWhiteTime = clock.WhiteRemainingTime;
            var actualBlackTime = clock.BlackRemainingTime;

            Assert.AreEqual(expectedWhiteTime, actualWhiteTime);
            Assert.AreEqual(expectedBlackTime, actualBlackTime);
        }

        [TestMethod]
        public void OnceOneOfTheTimersReachZeroTheButtonsStopWorkingAndBothTimersAreFrozen()
        {
            var whiteTime = TimeSpan.FromMilliseconds(100);
            var blackTime = TimeSpan.FromMilliseconds(200);
            var clock = new ChessClock(whiteTime, blackTime);

            clock.PressBlackStopButton();
            Thread.Sleep(whiteTime);
            var expectedWhiteTime = TimeSpan.Zero;
            var expectedBlackTime = blackTime;
            Thread.Sleep(100);
            clock.PressBlackStopButton();
            Thread.Sleep(100);
            clock.PressWhiteStopButton();
            Thread.Sleep(100);
            var actualWhiteTime = clock.WhiteRemainingTime;
            var actualBlackTime = clock.BlackRemainingTime;

            Assert.AreEqual(expectedWhiteTime, actualWhiteTime);
            Assert.AreEqual(expectedBlackTime, actualBlackTime);
        }
    }
}
