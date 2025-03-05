using Prism.Ioc;
using WebAdmin.Views;
using System.Windows;
using WebAdmin.ViewModels;
using Prism.Regions;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using WebAdmin.Db;
using WebAdmin.Units;

namespace WebAdmin;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    IContainerRegistry containerRegistry;
    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();

    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {


        containerRegistry.Register<WebDb>(c =>
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var qrFolder = Path.Combine(folder, appName);

            // 如果 IdentifyQRcodeDownload 文件夹不存在，则创建它
            if (!Directory.Exists(qrFolder))
                Directory.CreateDirectory(qrFolder);

            string DbPath = Path.Combine(qrFolder, $"{ConfigInfo.DbName}.db");

            var options = new DbContextOptionsBuilder<WebDb>()
            .UseSqlite($"Data Source={DbPath}")
            .Options;

            return new WebDb(options);
        });

        containerRegistry.RegisterForNavigation<WebStatusView, WebStatusViewModel>();

        this.containerRegistry = containerRegistry;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        // 获取 RegionManager
        var regionManager = Container.Resolve<IRegionManager>();

        // 在 "MainNavigationRegion" 这个区域自动导航到 MainView
        regionManager.RequestNavigate(NavName.MainNavigationRegion, nameof(WebStatusView));
    }
}
