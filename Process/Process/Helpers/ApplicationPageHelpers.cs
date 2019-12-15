using Process.Models.Common;
using Process.UI.Pages;

namespace Process.Helpers
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public static class ApplicationPageHelpers
    {
        /// <summary>
        /// Takes a <see cref="ApplicationPage"/> and a view model, if any, and creates the desired page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static BasePage ToBasePage(this ApplicationPage page, object viewModel = null)
        {
            switch (page)
            {
                case ApplicationPage.Dashboard:
                    return new Dashboard();

                case ApplicationPage.DiaryLog:
                    return new DiaryLog();

                case ApplicationPage.ToDoList:
                    return new ToDoLog();

                case ApplicationPage.WorkoutLog:
                    return new WorkoutLog();

                case ApplicationPage.PocketBank:
                    return new PocketBank();

                case ApplicationPage.BookLog:
                    return new BookLog();

                case ApplicationPage.Notebook:
                    return new Notebook();

                case ApplicationPage.AppSettings:
                    return new UI.Pages.AppSettings();

                default:
                    return new WelcomePage();
            }
        }

        /// <summary>
        /// Converts a <see cref="BasePage"/> to the specific <see cref="ApplicationPage"/> that is for that type of page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            // Alert developer of issue
            //Debugger.Break();
            return default(ApplicationPage);
        }
    }
}