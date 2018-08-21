using System;

namespace ChessClockApp
{
    public class NormalDelayChessClock : DelayChessClock
    {
        public NormalDelayChessClock(TimeSpan gameTime, TimeSpan delayTime) : base(gameTime, delayTime) { }

        protected override void UpdatePlayerRemainingTime(Player player)
        {
            if (TimerElapsedTime != TimeSpan.Zero)
                _remainingTime[player] -= TimerElapsedTime > _delayTime ? TimerElapsedTime : _delayTime;
            var otherPlayer = player == Player.ONE ? Player.TWO : Player.ONE;
            _remainingTime[otherPlayer] += _delayTime;
        }
    }
}