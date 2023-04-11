using DataAccessLibrary;
using System.Windows;

namespace WpfMultiWindow;

public partial class DataWindow : Window
{
    private readonly IDataAccess _dataAccess;

    public DataWindow(IDataAccess dataAccess)
    {
        InitializeComponent();
        _dataAccess = dataAccess;
        windowData.Text = _dataAccess.GetData();
    }
}
