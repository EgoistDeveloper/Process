﻿using Process.ViewModel.App;

namespace Process.UI.Pages
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : BasePage<WelcomeViewModel>
    {
        public WelcomePage() : base()
        {
            InitializeComponent();
        }

        public WelcomePage(WelcomeViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }
    }
}
