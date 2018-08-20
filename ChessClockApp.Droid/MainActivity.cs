using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using System.Timers;
using AlertDialog = Android.App.AlertDialog;

namespace ChessClockApp.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private const string TIME_FMT = @"mm\:ss";

        private readonly Timer _timer = new Timer(1000) { AutoReset = true, Enabled = false };

        private ChessClock _clock;
        private Button _playerOneBtn;
        private Button _playerTwoBtn;
        private AlertDialog _confirmResetDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _playerOneBtn = FindViewById<Button>(Resource.Id.playerOneButtonClock);
            _playerTwoBtn = FindViewById<Button>(Resource.Id.playerTwoButtonClock);
            var resetBtn = FindViewById<Button>(Resource.Id.resetButton);

            var gameTime = TimeSpan.FromMinutes(5);
            _clock = new NoDelayChessClock(gameTime);

            _playerOneBtn.Text = gameTime.ToString(TIME_FMT);
            _playerTwoBtn.Text = gameTime.ToString(TIME_FMT);

            _timer.Elapsed += Timer_Elapsed;
            _playerOneBtn.Click += PlayerOneBtn_Click;
            _playerTwoBtn.Click += PlayerTwoBtn_Click;
            resetBtn.Click += ResetBtn_Click;

            _confirmResetDialog = ((new AlertDialog.Builder(this))
                .SetMessage(Resource.String.confirm_reset_question)
                .SetPositiveButton(Resource.String.confirm_reset, ConfirmClockReset)
                .SetNegativeButton(Resource.String.cancel, (s, e) => { }))
                .Create();
        }

        private void PlayerOneBtn_Click(object sender, EventArgs e)
        {
            _clock.PressButton(Player.ONE);
            _playerOneBtn.Enabled = false;
            _playerTwoBtn.Enabled = true;
            _timer.Start();
        }

        private void PlayerTwoBtn_Click(object sender, EventArgs e)
        {
            _clock.PressButton(Player.TWO);
            _playerTwoBtn.Enabled = false;
            _playerOneBtn.Enabled = true;
            _timer.Start();
        }

        private void ResetBtn_Click(object sender, EventArgs e) => _confirmResetDialog.Show();

        private void ConfirmClockReset(object sender, EventArgs e)
        {
            _clock.Reset();
            _playerOneBtn.Enabled = true;
            _playerTwoBtn.Enabled = true;
            _timer.Stop();
            UpdateClock();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) => UpdateClock();

        private void UpdateClock()
        {
            _playerOneBtn.Text = _clock.GetRemainingTime(Player.ONE).ToString(TIME_FMT);
            _playerTwoBtn.Text = _clock.GetRemainingTime(Player.TWO).ToString(TIME_FMT);
            if (_clock.IsTimeUp())
            {
                _playerOneBtn.Enabled = false;
                _playerTwoBtn.Enabled = false;
                _timer.Stop();
            }
        }
    }
}