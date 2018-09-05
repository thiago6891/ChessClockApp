using System;

namespace ChessClock
{
    public class FischerDelayChessClock : DelayChessClock
    {
        public FischerDelayChessClock(TimeSpan gameTime, TimeSpan delayTime) : base(gameTime, delayTime) { }

        public override ClockSettings GetSettings()
        {
            return new ClockSettings(_gameTime, ClockSettings.DelayType.Fischer, _delayTime);
        }

        protected override void UpdatePlayerRemainingTime(Player player)
        {
            _remainingTime[player] -= TimerElapsedTime;
            var otherPlayer = player == Player.ONE ? Player.TWO : Player.ONE;
            _remainingTime[otherPlayer] += _delayTime;
        }
    }
}