﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Process.Data;
using Process.Dialogs.Pocket;
using Process.Helpers;
using Process.Models.Pocket;

namespace Process.ViewModel.PocketBank
{
    public class ActionInputViewModel : WindowViewModel
    {
        public ActionInputViewModel(Window window) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 500;
            Title = "Add Pocket Action";

            PocketAction = new PocketAction();

            CloseCommand = new RelayCommand(p =>
            {
                SaveAction();

                mWindow.Close();
            });

            AddCategoryCommand = new RelayCommand(p => AddCategory());
            LoadPocketCategories();
        }

        public ObservableCollection<PocketCategory> PocketCategories { get; set; } = new ObservableCollection<PocketCategory>();
        public PocketCategory SelectedPocketCategory { get; set; }

        public PocketAction PocketAction { get; set; }


        public void SaveAction()
        {
            if (SelectedPocketCategory == null || string.IsNullOrWhiteSpace(PocketAction.Title)) return;

            using var db = new AppDbContext();
            PocketAction.PocketCategoryId = SelectedPocketCategory.Id;

            db.PocketActions.Add(PocketAction);
            db.SaveChanges();
        }

        public ICommand AddCategoryCommand { get; set; }

        public void AddCategory()
        {
            var dialog = new ActionCategoryInput();

            dialog.Closing += (sender, args) =>
            {
                if (dialog.DataContext is CategoryInputViewModel vm && vm.PocketCategory.Id > 0)
                {
                    PocketCategories.Add(vm.PocketCategory);
                    SelectedPocketCategory = vm.PocketCategory;
                }
            };

            dialog.ShowDialogWindow(new CategoryInputViewModel(dialog), mWindow);
        }

        public void LoadPocketCategories()
        {
            using var db = new AppDbContext();
            PocketCategories = db.PocketCategories.ToObservableCollection();
            SelectedPocketCategory = PocketCategories.FirstOrDefault();
        }
    }
}
