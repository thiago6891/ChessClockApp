using System;

namespace ChessClock
{
    public class NoDelayChessClock : ChessClock
    {
        public NoDelayChessClock(TimeSpan gameTime) : base(gameTime) { }

        public override ClockSettings GetSettings()
        {
            return new ClockSettings(_gameTime, ClockSettings.DelayType.None, TimeSpan.Zero);
        }

        protected override void UpdatePlayerRemainingTime(Player player) => _remainingTime[player] -= TimerElapsedTime;
    }
}