using System;

namespace ChessClock
{
    public class ClockSettings
    {
        public enum DelayType
        {
            None,
            Fischer,
            Bronstein,
            Normal
        }

        public readonly TimeSpan GameTime;
        public readonly TimeSpan DelayTime;
        public readonly DelayType Delay;

        public ClockSettings(TimeSpan gameTime, DelayType delay, TimeSpan delayTime)
        {
            GameTime = gameTime;
            Delay = delay;
            DelayTime = delayTime;
        }
    }
}
