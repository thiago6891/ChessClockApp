using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace ChessClockApp.Droid
{
    public class SettingsDialogFragment : DialogFragment
    {
        private ISettingsButtonsListener _listener;

        public TimeSpan GameTime { get; private set; }

        public SettingsDialogFragment(TimeSpan gameTime) : base()
        {
            GameTime = gameTime;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.settings, null);
            view.FindViewById<EditText>(Resource.Id.gameTimeTextInput).Text = GameTime.ToString();

            return ((new AlertDialog.Builder(Activity))
                .SetView(view)
                .SetPositiveButton("Save", SaveButtonClick_Handler)
                .SetNegativeButton(Resource.String.cancel, (s, e) => { Dialog.Cancel(); }))
                .Create();
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

        private void SaveButtonClick_Handler(object sender, EventArgs e)
        {
            var gameTimeInput = Dialog.FindViewById<EditText>(Resource.Id.gameTimeTextInput);

            if (TimeSpan.TryParse(gameTimeInput.Text, out TimeSpan gameTime))
            {
                GameTime = gameTime;
            }
            else
            {
                // TODO: handle wrong format
            }

            _listener.OnSaveButtonClicked();
        }
    }
}