using DataAccessLibrary;
using System.Windows;
using WpfMultiWindow.Helpers;

namespace WpfMultiWindow;

public partial class MainWindow : Window
{
    private readonly IDataAccess _dataAccess;
    private readonly IAbstractFactory<DataWindow> _factory;

    public MainWindow(IDataAccess dataAccess, IAbstractFactory<DataWindow> factory)
    {
        InitializeComponent();
        _dataAccess = dataAccess;
        _factory = factory;
    }

    private void retrieveData_Click(object sender, RoutedEventArgs e)
    {
        data.Text = _dataAccess.GetData();
    }

    private void openWindow_Click(object sender, RoutedEventArgs e)
    {
        _factory.Create().Show();
    }
}
