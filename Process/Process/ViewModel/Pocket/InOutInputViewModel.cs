using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Process.Data;
using Process.Dialogs.Pocket;
using Process.Helpers;
using Process.Models.Pocket;
using Process.Models.Workout;

namespace Process.ViewModel.PocketBank
{
    public class InOutInputViewModel : WindowViewModel
    {
        public InOutInputViewModel(Window window) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 500;
            Title = "Add Pocket In/Out Log";

            PocketInOut = new PocketInOut();

            PocketInOutTypeItems = Enum.GetValues(typeof(PocketInOutType)).Cast<PocketInOutType>()
            .Select(x => new PocketInOutTypeItem()
            {
                PocketInOutType = x,
                PocketInOutTypeName = x.ToString()
            }).ToList();

            CloseCommand = new RelayCommand(p =>
            {
                SaveInOut();

                mWindow.Close();
            });

            AddPocketActionCommand = new RelayCommand(p => AddPocketAction());
            LoadActions();
        }

        #region Methods

        public ICommand AddPocketActionCommand { get; set; }

        #endregion

        #region Methods

        public PocketInOut PocketInOut { get; set; }
        public List<PocketInOutTypeItem> PocketInOutTypeItems { get; set; }
        public PocketInOutTypeItem SelectedPocketInOutTypeItem { get; set; }
        public ObservableCollection<PocketAction> PocketActions { get; set; } = new ObservableCollection<PocketAction>();
        public PocketAction SelectedPocketAction { get; set; }

        #endregion

        #region Methods

        public void SaveInOut()
        {
            if (PocketInOut.Amount > 0 && SelectedPocketAction != null && SelectedPocketInOutTypeItem != null)
            {
                using var db = new AppDbContext();
                PocketInOut.AddedDate = DateTime.Now;
                PocketInOut.Type = SelectedPocketInOutTypeItem.PocketInOutType;
                PocketInOut.PocketActionId = SelectedPocketAction.Id;

                db.PocketInOuts.Add(PocketInOut);
                db.SaveChanges();
            }
        }

        private void LoadActions()
        {
            using var db = new AppDbContext();
            PocketActions = db.PocketActions.ToObservableCollection();
            SelectedPocketAction = PocketActions.FirstOrDefault();
        }

        #endregion

        #region Command Methods

        public void AddPocketAction()
        {
            var dialog = new ActionInput();

            dialog.Closing += (sender, args) =>
            {
                if (dialog.DataContext is ActionInputViewModel vm && vm.PocketAction.Id > 0)
                {
                    PocketActions.Add(vm.PocketAction);
                    SelectedPocketAction = vm.PocketAction;
                }
            };

            dialog.ShowDialogWindow(new ActionInputViewModel(dialog), mWindow);
        }

        #endregion
    }
}