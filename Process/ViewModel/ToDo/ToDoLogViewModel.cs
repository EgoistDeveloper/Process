using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Process.Data;
using Process.Dialogs.ToDo;
using Process.Models.ToDo;
using Process.Dialogs;
using Process.ViewModel.App;
using Process.Helpers;

namespace Process.ViewModel.ToDo
{
    public class ToDoLogViewModel : ViewModelBase
    {
        public ToDoLogViewModel()
        {
            LoadToDoLists();

            AddToDoListCommand = new RelayCommand(p => AddToDoList());
            DeleteToDoListCommand = new RelayParameterizedCommand(DeleteToDoList);

            AddToDoCommand = new RelayParameterizedCommand(AddToDo);
            UpdateToDoCommand = new RelayParameterizedCommand(UpdateToDo);
            DeleteToDoListCommand = new RelayParameterizedCommand(DeleteToDoList);
            SetIsDoneCommand = new RelayParameterizedCommand(SetIsDone);
            SetIsImportantCommand = new RelayParameterizedCommand(SetIsImportant);
            DeleteToDoCommand = new RelayParameterizedCommand(DeleteToDo);
        }

        #region Commands

        // ToDo List Commands
        public ICommand AddToDoListCommand { get; set; }
        public ICommand DeleteToDoListCommand { get; set; }

        // ToDo Commands
        public ICommand AddToDoCommand { get; set; }
        public ICommand UpdateToDoCommand { get; set; }
        public ICommand SetIsDoneCommand { get; set; }
        public ICommand SetIsImportantCommand { get; set; }
        public ICommand DeleteToDoCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// ToDo list items
        /// </summary>
        public ObservableCollection<ToDoListItem> ToDoListItems { get; set; } = new ObservableCollection<ToDoListItem>();

        #endregion

        #region Methods

        /// <summary>
        /// Load todo lists
        /// </summary>
        public void LoadToDoLists()
        {
            using var db = new AppDbContext();

            ToDoListItems = db.ToDoLists.OrderByDescending(x => x.LastUpdateDate)
            .Select(x => new ToDoListItem()
            {
                ToDoList = new ToDoList
                {
                    Id = x.Id,
                    Title = x.Title,
                    LastUpdateDate = x.LastUpdateDate,
                    ToDos = new ObservableCollection<Models.ToDo.ToDo>(db.ToDos.Where(c => c.ToDoListId == x.Id)
                    .OrderByDescending(c => c.AddedDate).ToList())
                },
                ToDoDoneCount = db.ToDos.Count(c => c.ToDoListId == x.Id && c.IsDone),
                UpdateToDoListCommand = new RelayParameterizedCommand(UpdateToDoList)
            })
            .OrderBy(x => x.ToDoList.ToDos.Count > 0 && x.ToDoDoneCount == x.ToDoList.ToDos.Count)
            .ToObservableCollection();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Add todo list
        /// </summary>
        public void AddToDoList()
        {
            var dialog = new ToDoListInputDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is ToDoListInputViewModel vm && vm.ToDoListItem != null && vm.ToDoListItem.ToDoList.Id > 0)
                {
                    ToDoListItems.Insert(0, vm.ToDoListItem);
                }
            };

            dialog.ShowDialogWindow(new ToDoListInputViewModel(dialog));
        }

        /// <summary>
        /// Update ToDo List
        /// </summary>
        public void UpdateToDoList(object sender)
        {
            var toDoListItem = (sender as TextBlock).DataContext as ToDoListItem;

            var dialog = new ToDoListInputDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is ToDoListInputViewModel vm && vm.ToDoListItem != null && vm.ToDoListItem.ToDoList.Id > 0)
                {
                    toDoListItem = vm.ToDoListItem;
                }
            };

            dialog.ShowDialogWindow(new ToDoListInputViewModel(dialog, toDoListItem));
        }

        /// <summary>
        /// Delete ToDo List
        /// </summary>
        /// <param name="sender"></param>
        public void DeleteToDoList(object sender)
        {
            var toDoListItem = (sender as Button).DataContext as ToDoListItem;

            var dialog = new DeleteDialog();

            dialog.Closing += (send, args) =>
            {
                if (dialog.DataContext is DeleteDialogViewModel vm && vm.Result)
                {
                    using var db = new AppDbContext();

                    db.ToDoLists.Remove(toDoListItem.ToDoList);
                    db.SaveChanges();

                    ToDoListItems.Remove(toDoListItem);
                }
            };

            dialog.ShowDialogWindow(new DeleteDialogViewModel(dialog, "Delete Todo List", toDoListItem.ToDoList.Title));
        }

        /// <summary>
        /// Add ToDo
        /// </summary>
        /// <param name="sender"></param>
        public void AddToDo(object sender)
        {
            var toDoListItem = (sender as Button).DataContext as ToDoListItem;

            var dialog = new ToDoItemInputDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is ToDoItemInputViewModel vm && vm.ToDo != null && vm.ToDo.Id > 0)
                {
                    toDoListItem.ToDoList.ToDos.Insert(0, vm.ToDo);
                }
            };

            dialog.ShowDialogWindow(new ToDoItemInputViewModel(dialog, toDoListItem.ToDoList));
        }

        /// <summary>
        /// Update ToDo
        /// </summary>
        /// <param name="sender"></param>
        public void UpdateToDo(object sender)
        {
            var toDo = (sender as Button).DataContext as Models.ToDo.ToDo;

            var dialog = new ToDoItemInputDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is ToDoItemInputViewModel vm && vm.ToDo != null && vm.ToDo.Id > 0)
                {
                    toDo = vm.ToDo;
                }
            };

            dialog.ShowDialogWindow(new ToDoItemInputViewModel(dialog, null, toDo));
        }

        /// <summary>
        /// Set Is Done ToDo
        /// </summary>
        /// <param name="sender"></param>
        public void SetIsDone(object sender)
        {
            var toDo = (sender as CheckBox).DataContext as Models.ToDo.ToDo;

            using var db = new AppDbContext();

            db.ToDos.Update(toDo);
            db.SaveChanges();

            var toDoListItem = ToDoListItems.First(x => x.ToDoList.ToDos.Contains(toDo));

            toDoListItem.ToDoDoneCount = toDo.IsDone ? toDoListItem.ToDoDoneCount - 1 : toDoListItem.ToDoDoneCount + 1;
        }

        /// <summary>
        /// Set Is Important ToDo
        /// </summary>
        /// <param name="sender"></param>
        public void SetIsImportant(object sender)
        {
            var toDo = (sender as CheckBox).DataContext as Models.ToDo.ToDo;

            using var db = new AppDbContext();
            db.ToDos.Update(toDo);
            db.SaveChanges();
        }

        /// <summary>
        /// Delete ToDo
        /// </summary>
        /// <param name="sender"></param>
        public void DeleteToDo(object sender)
        {
            var toDo = (sender as Button).DataContext as Models.ToDo.ToDo;

            using var db = new AppDbContext();
            db.ToDos.Remove(toDo);
            db.SaveChanges();

            foreach (var toDoList in ToDoListItems)
            {
                if (!toDoList.ToDoList.ToDos.Contains(toDo)) continue;
                toDoList.ToDoList.ToDos.Remove(toDo);

                if (toDo.IsDone == true)
                    toDoList.ToDoDoneCount -= 1;
                break;
            }
        }

        #endregion
    }
}