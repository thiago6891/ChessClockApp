using System;
using System.Diagnostics;
using System.Timers;

namespace ChessClockApp
{
    public class ChessClock
    {
        private const int WHITE = 0;
        private const int BLACK = 1;
        private const int TOTAL_PLAYERS = 2;

        private readonly TimeSpan[] _totalTimes = new TimeSpan[TOTAL_PLAYERS];
        private readonly TimeSpan[] _increments = new TimeSpan[TOTAL_PLAYERS];
        private readonly Stopwatch[] _watches = new Stopwatch[TOTAL_PLAYERS];
        private readonly Timer[] _timers = new Timer[TOTAL_PLAYERS];

        private bool _gameOver;

        public TimeSpan WhiteRemainingTime { get => GetRemainingTime(WHITE); }
        public TimeSpan BlackRemainingTime { get => GetRemainingTime(BLACK); }

        public ChessClock(TimeSpan whiteTime, TimeSpan blackTime)
        {
            _totalTimes[WHITE] = whiteTime;
            _totalTimes[BLACK] = blackTime;
            
            for (int p = WHITE; p < TOTAL_PLAYERS; p++)
            {
                _watches[p] = new Stopwatch();
                _timers[p] = new Timer(GetRemainingTime(p).TotalMilliseconds);
                _timers[p].Elapsed += TimesUpEventHandler;
            }

            _gameOver = false;
        }

        public ChessClock(TimeSpan whiteTime, TimeSpan blackTime, TimeSpan whiteIncrement, TimeSpan blackIncrement)
            : this(whiteTime, blackTime)
        {
            _increments[WHITE] = whiteIncrement;
            _increments[BLACK] = blackIncrement;
        }

        public void PressBlackStopButton()
        {
            bool GameStarted = _gameOver || _watches[WHITE].IsRunning || _watches[BLACK].IsRunning;
            if (!GameStarted)
            {
                _watches[WHITE].Start();
                _timers[WHITE].Start();
            }
            else
            {
                PressStopButton(BLACK);
            }
        }

        public void PressWhiteStopButton()
        {
            PressStopButton(WHITE);
        }

        public void Stop()
        {
            foreach (var timer in _timers)
            {
                timer.Stop();
                timer.Dispose();
            }
            foreach (var watch in _watches) watch.Stop();
            _gameOver = true;
        }

        private void PressStopButton(int player)
        {
            var otherPlayer = (player + 1) % TOTAL_PLAYERS;
            if (!_gameOver && _watches[player].IsRunning)
            {
                _watches[player].Stop();
                _timers[player].Stop();
                _timers[player].Dispose();

                _watches[otherPlayer].Start();
                _timers[otherPlayer].Start();

                _totalTimes[player] += _increments[player];
                _timers[player] = new Timer(GetRemainingTime(player).TotalMilliseconds);
                _timers[player].Elapsed += TimesUpEventHandler;
            }
        }

        private void TimesUpEventHandler(object sender, ElapsedEventArgs e)
        {
            Stop();
        }

        private TimeSpan GetRemainingTime(int player)
        {
            return GetRemainingTime(_totalTimes[player], _watches[player].Elapsed);
        }

        private TimeSpan GetRemainingTime(TimeSpan total, TimeSpan elapsed)
        {
            if (total - elapsed <= TimeSpan.Zero) return TimeSpan.Zero;
            return total - elapsed;
        }
    }
}
