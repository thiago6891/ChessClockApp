using Android.App;
using Android.Content;
using Android.OS;

namespace ChessClockApp.Droid
{
    public class ResetClockDialogFragment : DialogFragment
    {
        private IOnClockResetConfirmedListener _listener;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return ((new AlertDialog.Builder(Activity))
                .SetMessage(Resource.String.confirm_reset_question)
                .SetPositiveButton(Resource.String.confirm_reset, (s, e) => _listener.OnClockResetConfirmed())
                .SetNegativeButton(Resource.String.cancel, (s, e) => { }))
                .Create();
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            _listener = (IOnClockResetConfirmedListener)context;
        }

        public interface IOnClockResetConfirmedListener
        {
            void OnClockResetConfirmed();
        }
    }
}