using System;

namespace ChessClock
{
    public class BronsteinDelayChessClock : DelayChessClock
    {
        public BronsteinDelayChessClock(TimeSpan gameTime, TimeSpan delayTime) : base(gameTime, delayTime)
        {
            _remainingTime[Player.ONE] += _delayTime;
            _remainingTime[Player.TWO] += _delayTime;
        }

        protected override void UpdatePlayerRemainingTime(Player player)
        {
            if (TimerElapsedTime > _delayTime)
                _remainingTime[player] += _delayTime - TimerElapsedTime;
        }
    }
}