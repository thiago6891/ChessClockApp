using System;

namespace ChessClockApp
{
    public class FischerDelayChessClock : DelayChessClock
    {
        public FischerDelayChessClock(TimeSpan gameTime, TimeSpan delayTime) : base(gameTime, delayTime) { }

        protected override void UpdatePlayerRemainingTime(Player player) =>
            _remainingTime[player] += _delayTime - TimerElapsedTime;
    }
}