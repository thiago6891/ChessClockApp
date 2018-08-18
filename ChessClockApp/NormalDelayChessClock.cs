using System;

namespace ChessClockApp
{
    public class NormalDelayChessClock : DelayChessClock
    {
        public NormalDelayChessClock(TimeSpan gameTime, TimeSpan delayTime) : base(gameTime, delayTime) { }

        protected override void UpdatePlayerRemainingTime(Player player)
        {
            if (TimerElapsedTime > _delayTime)
                _remainingTime[player] += _delayTime - TimerElapsedTime;
        }
    }
}