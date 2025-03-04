using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Navigation;
using WebAdmin.Units;
using WebAdmin.Views;

namespace WebAdmin.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private string _title = "WebAdmin";
    private readonly IRegionManager regionManager;
    public string Title
    {
        get { return _title; }
        set { SetProperty(ref _title, value); }
    }

    public MainWindowViewModel(IRegionManager regionManager)
    {
        this.regionManager = regionManager;
    }
}
