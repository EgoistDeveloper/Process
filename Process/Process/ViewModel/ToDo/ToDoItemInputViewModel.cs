using System.Windows;
using Process.Data;
using Process.Models.ToDo;

namespace Process.ViewModel.ToDo
{
    public class ToDoItemInputViewModel : WindowViewModel
    {
        /// <summary>
        /// Current constructor
        /// </summary>
        /// <param name="window">Parent window</param>
        /// <param name="toDoList">ToDo List Id</param>
        public ToDoItemInputViewModel(Window window, ToDoList toDoList, Models.ToDo.ToDo toDo = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 600;
            Title = toDo != null ? $"Edit: {toDo.ToDoContent}" : "Add new ToDo";

            ToDoList = toDoList;
            ToDo = toDo ?? new Models.ToDo.ToDo();

            CloseCommand = new RelayCommand(p => AddOrUpdate());
        }

        #region Properties

        public ToDoList ToDoList { get; set; }
        public Models.ToDo.ToDo ToDo { get; set; }

        #endregion

        #region Command Method

        /// <summary>
        /// Save and close current window
        /// </summary>
        private void AddOrUpdate()
        {
            if (!string.IsNullOrWhiteSpace(ToDo.ToDoContent))
            {
                using var db = new AppDbContext();
                
                if (ToDo.Id > 0)
                {
                    db.ToDos.Update(ToDo);
                }
                else
                {
                    ToDo.ToDoListId = ToDoList.Id;

                    db.ToDos.Add(ToDo);
                }

                db.SaveChanges();
            }

            mWindow.Close();
        }

        #endregion
    }
}