using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace ChessClockApp.Droid
{
    public enum Delay
    {
        None,
        Fischer,
        Bronstein,
        Normal
    }

    public class SettingsDialogFragment : DialogFragment
    {
        private EditText _gameTimeInput;
        private EditText _delayTimeInput;
        private RadioGroup _delayRadioGroup;

        private ISettingsButtonsListener _listener;

        public TimeSpan GameTime { get; private set; }
        public TimeSpan DelayTime { get; private set; }
        public Delay DelayType { get; private set; }

        public SettingsDialogFragment(TimeSpan gameTime) : base()
        {
            GameTime = gameTime;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.settings, null);

            _gameTimeInput = view.FindViewById<EditText>(Resource.Id.gameTimeTextInput);
            _gameTimeInput.Text = GameTime.ToString();

            _delayTimeInput = view.FindViewById<EditText>(Resource.Id.delayTimeTextInput);
            _delayRadioGroup = view.FindViewById<RadioGroup>(Resource.Id.delayRadioGroup);

            view.FindViewById<RadioButton>(Resource.Id.noDelayButton).Click += NoDelayButton_Click;
            view.FindViewById<RadioButton>(Resource.Id.fischerDelayButton).Click += DelayButton_Click;
            view.FindViewById<RadioButton>(Resource.Id.bronsteinDelayButton).Click += DelayButton_Click;
            view.FindViewById<RadioButton>(Resource.Id.normalDelayButton).Click += DelayButton_Click;

            return ((new AlertDialog.Builder(Activity))
                .SetView(view)
                .SetPositiveButton("Save", SaveButtonClick_Handler)
                .SetNegativeButton(Resource.String.cancel, (s, e) => { Dialog.Cancel(); }))
                .Create();
        }

        private void SaveButtonClick_Handler(object sender, EventArgs e)
        {
            if (_delayTimeInput.Enabled)
            {
                if (TimeSpan.TryParse(_gameTimeInput.Text, out TimeSpan gameTime) &&
                    TimeSpan.TryParse(_delayTimeInput.Text, out TimeSpan delayTime))
                {
                    GameTime = gameTime;
                    DelayTime = delayTime;
                    switch (_delayRadioGroup.CheckedRadioButtonId)
                    {
                        case Resource.Id.fischerDelayButton:
                            DelayType = Delay.Fischer;
                            break;
                        case Resource.Id.bronsteinDelayButton:
                            DelayType = Delay.Bronstein;
                            break;
                        case Resource.Id.normalDelayButton:
                            DelayType = Delay.Normal;
                            break;
                    }
                }
                else
                {
                    // TODO: handle wrong format
                }
            }
            else
            {
                if (TimeSpan.TryParse(_gameTimeInput.Text, out TimeSpan gameTime))
                {
                    GameTime = gameTime;
                    DelayTime = TimeSpan.Zero;
                    DelayType = Delay.None;
                }
                else
                {
                    // TODO: handle wrong format
                }
            }

            _listener.OnSaveButtonClicked();
        }

        private void NoDelayButton_Click(object sender, EventArgs e)
        {
            _delayTimeInput.Enabled = false;
        }

        private void DelayButton_Click(object sender, EventArgs e)
        {
            _delayTimeInput.Enabled = true;
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            _listener = (ISettingsButtonsListener)context;
        }

        public interface ISettingsButtonsListener
        {
            void OnSaveButtonClicked();
        }
    }
}