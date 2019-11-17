using Process.Models.Common;
using Process.UI.Pages;
using Process.ViewModel.App;
using Process.ViewModel.Book;
using Process.ViewModel.Dashboard;
using Process.ViewModel.Diary;
using Process.ViewModel.Diet;
using Process.ViewModel.Notebook;
using Process.ViewModel.PocketBank;
using Process.ViewModel.ToDo;
using Process.ViewModel.Workout;

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

                case ApplicationPage.DietLog:
                    return new DietLog();

                case ApplicationPage.BookLog:
                    return new BookLog();

                case ApplicationPage.Notebook:
                    return new Notebook();
                default:
                    // Debugger.Break();
                    return null;
            }
        }

        /// <summary>
        /// Converts a <see cref="BasePage"/> to the specific <see cref="ApplicationPage"/> that is for that type of page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            //// Find application page that matches the base page
            //if (page is ChatPage)
            //    return ApplicationPage.Chat;

            //if (page is LoginPage)
            //    return ApplicationPage.Login;

            //if (page is RegisterPage)
            //    return ApplicationPage.Register;

            // Alert developer of issue
            //Debugger.Break();
            return default(ApplicationPage);
        }
    }

}
