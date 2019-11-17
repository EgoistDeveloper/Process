using GalaSoft.MvvmLight;

namespace Process.ViewModel.Dialog
{
    /// <summary>
    /// A base view model for any dialogs
    /// </summary>
    public class BaseDialogViewModel : ViewModelBase
    {
        /// <summary>
        /// The title of the dialog
        /// </summary>
        public string Title { get; set; }
    }
}
