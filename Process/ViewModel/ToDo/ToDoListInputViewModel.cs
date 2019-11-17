using System;
using System.Windows;
using Process.Data;
using Process.Models.ToDo;

namespace Process.ViewModel.ToDo
{
    public class ToDoListInputViewModel : WindowViewModel
    {
        /// <summary>
        /// Current constructor
        /// </summary>
        /// <param name="window">Parent window</param>
        public ToDoListInputViewModel(Window window, ToDoListItem toDoListItem = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 600;
            Title = toDoListItem != null ? $"Edit {toDoListItem.ToDoList.Title}" : "Add New ToDo List";

            ToDoListItem = toDoListItem ?? new ToDoListItem();

            CloseCommand = new RelayCommand(p => AddOrUpdate());
        }

        #region Properties

        public ToDoListItem ToDoListItem { get; set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Add or Update
        /// </summary>
        private void AddOrUpdate()
        {
            using var db = new AppDbContext();

            if (string.IsNullOrWhiteSpace(ToDoListItem.ToDoList.Title))
            {
                if (ToDoListItem.ToDoList.Id > 0)
                {
                    db.ToDoLists.Update(ToDoListItem.ToDoList);
                }
                else
                {
                    ToDoListItem.ToDoList.LastUpdateDate = DateTime.Now;

                    db.ToDoLists.Add(ToDoListItem.ToDoList);
                }
            }
            
            db.SaveChanges();

            mWindow.Close();
        }

        #endregion
    }
}