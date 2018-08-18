using System;

namespace ChessClockApp
{
    public class NoDelayChessClock : ChessClock
    {
        public NoDelayChessClock(TimeSpan gameTime) : base(gameTime) { }

        protected override void UpdatePlayerRemainingTime(Player player) => _remainingTime[player] -= TimerElapsedTime;
    }
}