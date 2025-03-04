using Microsoft.EntityFrameworkCore;
using Microsoft.Xaml.Behaviors.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebAdmin.Db;
using WebAdmin.Models;
using WebAdmin.Units;

namespace WebAdmin.ViewModels;

public class WebStatusViewModel : BindableBase
{
    public readonly WebDb webDb;
    private bool _isOpen;

    public bool IsOpen
    {
        get { return _isOpen; }
        set { SetProperty(ref _isOpen, value); }
    }


    private ObservableCollection<SiteModel> _siteModels;
    public ObservableCollection<SiteModel> SiteModels
    {
        get { return _siteModels; }
        set { SetProperty(ref _siteModels, value); } // 触发 UI 更新
    }

    public WebStatusViewModel(WebDb webDb)
    {
        this.webDb = webDb;
        _ = GetDbAsync();
        _ = updateWebStatus();
    }

    private async Task GetDbAsync()
    {
        IsOpen = true;
        SiteModels = new ObservableCollection<SiteModel>(await webDb.SiteModels.ToListAsync()); // 异步加载数据库数据
        IsOpen = false;
    }

    /// <summary>
    /// 刷新网站状态
    /// </summary>
    private async Task updateWebStatus()
    {
        IsOpen = true;
        await Task.Delay(500); // 模拟任务，防止 UI 闪烁
        var updateTasks = SiteModels.Select(site => site.UpdateStatus());
        await Task.WhenAll(updateTasks);
        IsOpen = false;
    }

    public async Task RefreshDataAsync()
    {
        IsOpen = true;

        var sites = await webDb.SiteModels.ToListAsync();
        SiteModels.Clear();

        // 创建所有的 UpdateStatus 任务
        var updateTasks = sites.Select(site => site.UpdateStatus()).ToList();

        // 并发执行所有 UpdateStatus 任务
        await Task.WhenAll(updateTasks);

        // 更新 UI
        foreach (var site in sites)
        {
            SiteModels.Add(site);
        }

        IsOpen = false;
    }
    private DelegateCommand _updateCommand;
    public DelegateCommand UpdateCommand => _updateCommand ??= new DelegateCommand(async () => await RefreshDataAsync());


}
