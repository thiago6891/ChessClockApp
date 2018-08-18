using System;
using System.Collections.Generic;
using System.Threading;

namespace ChessClockApp
{
    public enum Player
    {
        ONE,
        TWO
    }

    public enum TimeControl
    {
        FISCHER,
        BRONSTEIN,
        DELAY
    }

    public class ChessClock
    {
        private readonly TimeSpan _gameTime;
        private readonly TimeSpan _delayTime;
        private readonly TimeControl? _timeControl;
        private readonly Dictionary<Player, TimeSpan> _remainingTime = new Dictionary<Player, TimeSpan>(2)
        {
            { Player.ONE, TimeSpan.Zero },
            { Player.TWO, TimeSpan.Zero }
        };

        private TimeSpan TimerElapsedTime => _lastTimerStart.HasValue ? 
            DateTime.Now - _lastTimerStart.Value : TimeSpan.Zero;

        private Timer _countdownTimer;
        private DateTime? _lastTimerStart;
        private Player? _currentPlayer;

        public ChessClock(TimeSpan gameTime) : this(gameTime, TimeSpan.Zero, null) { }

        public ChessClock(TimeSpan gameTime, TimeSpan delayTime, TimeControl? timeControl)
        {
            _gameTime = gameTime;
            _remainingTime[Player.ONE] = gameTime;
            _remainingTime[Player.TWO] = gameTime;

            if (timeControl.HasValue)
            {
                _delayTime = delayTime;
                _timeControl = timeControl;
                if (_timeControl.Value == TimeControl.BRONSTEIN)
                {
                    _remainingTime[Player.ONE] += _delayTime;
                    _remainingTime[Player.TWO] += _delayTime;
                }
            }
            else
            {
                _delayTime = TimeSpan.Zero;
                _timeControl = null;
            }

            _countdownTimer = new Timer(TimeUp);
            _countdownTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void PressButton(Player player)
        {
            if (ChangePlayer(player))
            {
                UpdatePlayerRemainingTime(player);
                ResetTimer();
            }
        }

        public TimeSpan GetRemainingTime(Player player)
        {
            var elapsedTime = TimeSpan.Zero;
            if (player == _currentPlayer)
                elapsedTime = TimerElapsedTime;
            return _remainingTime[player] - elapsedTime;
        }

        public void Reset()
        {
            _remainingTime[Player.ONE] = _gameTime;
            _remainingTime[Player.TWO] = _gameTime;
            _currentPlayer = null;
            _lastTimerStart = null;
            _countdownTimer.Dispose();
            _countdownTimer = new Timer(TimeUp);
            _countdownTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private bool ChangePlayer(Player from)
        {
            var otherPlayer = from == Player.ONE ? Player.TWO : Player.ONE;
            if (_currentPlayer == otherPlayer)
                return false;

            _currentPlayer = otherPlayer;
            return true;
        }

        private void ResetTimer()
        {
            _countdownTimer.Dispose();
            _countdownTimer = new Timer(TimeUp);
            _countdownTimer.Change(_remainingTime[_currentPlayer.Value], TimeSpan.FromMilliseconds(-1));
            _lastTimerStart = DateTime.Now;
        }

        private void TimeUp(object state)
        {
            _remainingTime[_currentPlayer.Value] = TimeSpan.Zero;
            _currentPlayer = null;
            _lastTimerStart = null;
            _countdownTimer.Dispose();
            _countdownTimer = new Timer(TimeUp);
            _countdownTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void UpdatePlayerRemainingTime(Player player)
        {
            if (_timeControl.HasValue)
            {
                if (_timeControl.Value == TimeControl.FISCHER)
                    _remainingTime[player] += _delayTime - TimerElapsedTime;

                if (_timeControl.Value == TimeControl.BRONSTEIN || _timeControl.Value == TimeControl.DELAY)
                    if (TimerElapsedTime > _delayTime)
                        _remainingTime[player] += _delayTime - TimerElapsedTime;
            }
            else
            {
                _remainingTime[player] -= TimerElapsedTime;
            }
        }
    }
}