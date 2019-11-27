using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Process.Data;
using Process.Models.Pocket;

namespace Process.ViewModel.PocketBank
{
    public class CategoryInputViewModel : WindowViewModel
    {
        public CategoryInputViewModel(Window window) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 500;
            Title = "Add Pocket Action Category";

            PocketCategoryPriorityItems = Enum.GetValues(typeof(PocketCategoryPriority)).Cast<PocketCategoryPriority>()
            .Select(x => new PocketCategoryPriorityItem()
            {
                PocketCategoryPriority = x,
                PocketCategoryPriorityName = x.ToString()
            }).ToList();

            PocketCategory = new PocketCategory();

            CloseCommand = new RelayCommand(p =>
            {
                SaveCategory();

                mWindow.Close();
            });
        }

        #region Properties

        public List<PocketCategoryPriorityItem> PocketCategoryPriorityItems { get; set; }
        public PocketCategoryPriorityItem SelectedPocketCategoryPriorityItem { get; set; }
        public PocketCategory PocketCategory { get; set; }

        #endregion

        #region Methods

        public void SaveCategory()
        {
            if (string.IsNullOrWhiteSpace(PocketCategory.Title) || SelectedPocketCategoryPriorityItem == null) return;

            using var db = new AppDbContext();
            PocketCategory.Priority = SelectedPocketCategoryPriorityItem.PocketCategoryPriority;

            db.PocketCategories.Add(PocketCategory);
            db.SaveChanges();
        }

        #endregion
    }
}
