using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Xaml.Behaviors.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WebAdmin.Db;
using WebAdmin.Models;
using WebAdmin.Units;
using static System.Net.Mime.MediaTypeNames;

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

    private string _messageString;

    public string MessageString
    {
        get { return _messageString; }
        set { SetProperty(ref _messageString, value); }
    }

    private int _timeValue;

    public int TimeValue
    {
        get { return _timeValue; }
        set { SetProperty(ref _timeValue, value); }
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
        StartTimer();
    }
    Timer _timer;

    private void StartTimer()
    {
        _timer = new Timer(1000); // 设置定时间隔为1秒
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        TimeValue++;
    }

    private async Task GetDbAsync()
    {
        IsOpen = true;
        var list = await webDb.SiteModels.ToListAsync();
        list.ForEach(site => site.Description = string.Empty);
        await webDb.SaveChangesAsync(); 

        SiteModels = new ObservableCollection<SiteModel>(await webDb.SiteModels.ToListAsync()); // 异步加载数据库数据
        IsOpen = false;
    }

    private DelegateCommand<object> _openUrlCommand;
    public DelegateCommand<object> OpenUrlCommand => _openUrlCommand ??= new DelegateCommand<object>(ExecuteOpenUrlCommand);

    private void ExecuteOpenUrlCommand(object obj)
    {
        if (obj is not SiteModel siteModel)
        {
            MessageString = "未知的网站";
            return;
        }
        string newUrl = Tools.CorrectWebsite(siteModel.Address);
        MessageString = Tools.OpenUrl(newUrl);
    }


    private DelegateCommand<object> _copyCommand;
    public DelegateCommand<object> CopyCommand => _copyCommand ??= new DelegateCommand<object>(ExecuteCopyCommand);

    void ExecuteCopyCommand(object obj)
    {
        if (obj is not SiteModel siteModel)
        {
            MessageString = "未知的网站";
            return;
        }

        if (string.IsNullOrEmpty(siteModel.Address)) return;

        Clipboard.SetDataObject(siteModel.Address);
        MessageString = $"复制成功：{siteModel.Address}";

    }





    public async Task RefreshDataAsync()
    {
        TimeValue = 0;
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
