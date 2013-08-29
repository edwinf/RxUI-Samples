using ConfigTool.ViewModel;
using ReactiveUI;
using ReactiveUI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConfigTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        public MainWindow()
        {
            this.ViewModel = new MainWindowViewModel();

            InitializeComponent();
            
            this.BindCommand(this.ViewModel, x => x.Refresh);
            this.Bind(this.ViewModel, x => x.SearchFilter);

            this.OneWayBind(this.ViewModel, vm => vm.ConfigurationList, w => w.ConfigurationList.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.IsBusy, w => w.IsBusy.Visibility, () => false, BooleanToVisibilityHint.None);
            this.OneWayBind(this.ViewModel, vm => vm.IsNotBusy, w => w.ConfigurationList.IsEnabled, () => true, null, null);
        }

        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MainWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainWindowViewModel)value; }
        }
    }
}
