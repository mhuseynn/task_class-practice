using System.Windows.Navigation;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : NavigationWindow
    {
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
