using GalaSoft.MvvmLight;
using Process.Models.Common;
using System.Windows.Input;

namespace Process.ViewModel.App
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// True if the settings menu should be shown
        /// </summary>
        private bool mSettingsMenuVisible;

        #endregion

        #region Public Properties

        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.WelcomePage;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public ViewModelBase CurrentPageViewModel { get; set; }

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        public bool SideMenuVisible { get; set; } = false;

        /// <summary>
        /// True if the settings menu should be shown
        /// </summary>
        public bool SettingsMenuVisible
        {
            get => mSettingsMenuVisible;
            set
            {
                // If property has not changed...
                if (mSettingsMenuVisible == value)
                    // Ignore
                    return;

                // Set the backing field
                mSettingsMenuVisible = value;

                // If the settings menu is now visible...
                //if (value)
                //    // Reload settings
                //    TaskManager.RunAndForget(ViewModelSettings.LoadAsync);
            }
        }


        /// <summary>
        /// Determines if the application has network access to the fasetto server
        /// </summary>
        public bool ServerReachable { get; set; } = true;

        #endregion


        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public ApplicationViewModel()
        {
        }

        #endregion


        #region Public Helper Methods

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, ViewModelBase viewModel = null)
        {
            // Always hide settings page if we are changing pages
            //SettingsMenuVisible = false;

            // Set the view model
            CurrentPageViewModel = viewModel;

            // See if page has changed
            var different = CurrentPage != page;

            // Set the current page
            CurrentPage = page;

            // If the page hasn't changed, fire off notification
            // So pages still update if just the view model has changed
            if (!different)
                OnPropertyChanged(nameof(CurrentPage));

            // Show side menu or not?
            SideMenuVisible = page == ApplicationPage.WelcomePage;

        }

        ///// <summary>
        ///// Handles what happens when we have successfully logged in
        ///// </summary>
        ///// <param name="loginResult">The results from the successful login</param>
        //public async Task HandleSuccessfulLoginAsync(UserProfileDetailsApiModel loginResult)
        //{
        //    // Store this in the client data store
        //    await ClientDataStore.SaveLoginCredentialsAsync(loginResult.ToLoginCredentialsDataModel());

        //    // Load new settings
        //    await ViewModelSettings.LoadAsync();

        //    // Go to chat page
        //    ViewModelApplication.GoToPage(ApplicationPage.Chat);
        //}

        #endregion
    }
}
