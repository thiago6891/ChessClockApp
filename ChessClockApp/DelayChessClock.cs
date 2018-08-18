using System;

namespace ChessClockApp
{
    public abstract class DelayChessClock : ChessClock
    {
        protected readonly TimeSpan _delayTime;

        public DelayChessClock(TimeSpan gameTime, TimeSpan delayTime) : base(gameTime) => _delayTime = delayTime;
    }
}