using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Timers;

namespace ChessClockApp.Droid
{
    [Activity(Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : 
        AppCompatActivity, 
        ResetClockDialogFragment.IOnClockResetConfirmedListener, 
        SettingsDialogFragment.ISettingsButtonsListener
    {
        private readonly Timer _timer = new Timer(1000) { AutoReset = true, Enabled = false };
        private readonly ResetClockDialogFragment _resetClockDialog = new ResetClockDialogFragment();
        private readonly SettingsDialogFragment _settingsDialog = new SettingsDialogFragment();

        private ChessClock _clock;
        private Button _playerOneBtn;
        private Button _playerTwoBtn;
        private Button _resetBtn;
        private Button _settingsBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _playerOneBtn = FindViewById<Button>(Resource.Id.playerOneButtonClock);
            _playerTwoBtn = FindViewById<Button>(Resource.Id.playerTwoButtonClock);
            _resetBtn = FindViewById<Button>(Resource.Id.resetButton);
            _settingsBtn = FindViewById<Button>(Resource.Id.settingsButton);
            
            _clock = new NoDelayChessClock(_settingsDialog.GameTime);

            _playerOneBtn.Text = GetFormattedTime(Player.ONE);
            _playerTwoBtn.Text = GetFormattedTime(Player.TWO);

            _timer.Elapsed += Timer_Elapsed;
            _playerOneBtn.Click += PlayerOneBtn_Click;
            _playerTwoBtn.Click += PlayerTwoBtn_Click;
            _resetBtn.Click += ResetBtn_Click;
            _settingsBtn.Click += SettingsBtn_Click;

            _resetBtn.Visibility = ViewStates.Gone;
        }

        private void PlayerOneBtn_Click(object sender, EventArgs e)
        {
            _clock.PressButton(Player.ONE);
            _playerOneBtn.Enabled = false;
            _playerTwoBtn.Enabled = true;
            _settingsBtn.Visibility = ViewStates.Gone;
            _resetBtn.Visibility = ViewStates.Visible;
            _timer.Start();
        }

        private void PlayerTwoBtn_Click(object sender, EventArgs e)
        {
            _clock.PressButton(Player.TWO);
            _playerTwoBtn.Enabled = false;
            _playerOneBtn.Enabled = true;
            _settingsBtn.Visibility = ViewStates.Gone;
            _resetBtn.Visibility = ViewStates.Visible;
            _timer.Start();
        }

        private void ResetBtn_Click(object sender, EventArgs e) => _resetClockDialog.Show(FragmentManager, "");

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            _settingsDialog.Show(FragmentManager, "");
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) => UpdateClock();

        private void UpdateClock()
        {
            _playerOneBtn.Text = GetFormattedTime(Player.ONE);
            _playerTwoBtn.Text = GetFormattedTime(Player.TWO);
            if (_clock.IsTimeUp())
            {
                _playerOneBtn.Enabled = false;
                _playerTwoBtn.Enabled = false;
                _timer.Stop();
            }
        }

        private string GetFormattedTime(Player player)
        {
            var time = _clock.GetRemainingTime(player);
            if (time.TotalHours >= 1)
                return time.ToString(@"hh\:mm\:ss");
            return time.ToString(@"mm\:ss");
        }

        public void OnClockResetConfirmed()
        {
            _clock.Reset();
            _playerOneBtn.Enabled = true;
            _playerTwoBtn.Enabled = true;
            _timer.Stop();
            UpdateClock();
            _settingsBtn.Visibility = ViewStates.Visible;
            _resetBtn.Visibility = ViewStates.Gone;
        }

        public void OnSaveButtonClicked()
        {
            switch (_settingsDialog.DelayType)
            {
                case Delay.None:
                    _clock = new NoDelayChessClock(_settingsDialog.GameTime);
                    break;
                case Delay.Fischer:
                    _clock = new FischerDelayChessClock(_settingsDialog.GameTime, _settingsDialog.DelayTime);
                    break;
                case Delay.Bronstein:
                    _clock = new BronsteinDelayChessClock(_settingsDialog.GameTime, _settingsDialog.DelayTime);
                    break;
                case Delay.Normal:
                    _clock = new NormalDelayChessClock(_settingsDialog.GameTime, _settingsDialog.DelayTime);
                    break;
            }

            _playerOneBtn.Text = GetFormattedTime(Player.ONE);
            _playerTwoBtn.Text = GetFormattedTime(Player.TWO);
        }
    }
}