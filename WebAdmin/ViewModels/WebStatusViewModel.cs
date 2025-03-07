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

namespace WebAdmin.ViewModels;

public class WebStatusViewModel : BindableBase
{
    private Visibility _dataGridVisibility = Visibility.Collapsed;

    public Visibility DataGridVisibility
    {
        get { return _dataGridVisibility; }
        set { SetProperty(ref _dataGridVisibility, value); }
    }

    private Visibility _cardListVisibility = Visibility.Visible ;

    public Visibility CardListVisibility
    {
        get { return _cardListVisibility; }
        set { SetProperty(ref _cardListVisibility, value); }
    }

    private DelegateCommand<string> _sitchCardAndDataGridCommand;
    public DelegateCommand<string> SitchCardAndDataGridCommand =>
        _sitchCardAndDataGridCommand ?? (_sitchCardAndDataGridCommand = new DelegateCommand<string>(ExecuteSitchCardAndDataGridCommand));

    void ExecuteSitchCardAndDataGridCommand(string flag)
    {
        if(flag == "Card")
        {
            CardListVisibility = Visibility.Visible;
            DataGridVisibility = Visibility.Collapsed;
        }
        else
        {
            CardListVisibility = Visibility.Collapsed;
            DataGridVisibility = Visibility.Visible;
        }
    }



    public readonly WebDb webDb;
    private bool _isOpen;

    /// <summary>
    /// 弹窗是否打开
    /// </summary>
    public bool IsOpen
    {
        get { return _isOpen; }
        set { SetProperty(ref _isOpen, value); }
    }

    private string _messageString;

    /// <summary>
    /// 消息字符串
    /// </summary>
    public string MessageString
    {
        get { return _messageString; }
        set { SetProperty(ref _messageString, value); }
    }

    private int _timeValue;

    /// <summary>
    /// 时间值
    /// </summary>
    public int TimeValue
    {
        get { return _timeValue; }
        set { SetProperty(ref _timeValue, value); }
    }

    private ObservableCollection<SiteModel> _siteModels;
    /// <summary>
    /// 网站集合
    /// </summary>
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

    /// <summary>
    /// 开始计时
    /// </summary>
    private void StartTimer()
    {
        _timer = new Timer(1000); // 设置定时间隔为1秒
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }
    /// <summary>
    /// 定时器事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        TimeValue++;
    }

    /// <summary>
    /// 从数据库中获取数据
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 打开网址命令
    /// </summary>
    public DelegateCommand<object> OpenUrlCommand => _openUrlCommand ??= new DelegateCommand<object>(ExecuteOpenUrlCommand);

    /// <summary>
    /// 打开网址命令
    /// </summary>
    /// <param name="obj"></param>
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
    /// <summary>
    /// 复制网址命令
    /// </summary>
    public DelegateCommand<object> CopyCommand => _copyCommand ??= new DelegateCommand<object>(ExecuteCopyCommand);

    /// <summary>
    /// 复制网址
    /// </summary>
    /// <param name="obj"></param>
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

    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <returns></returns>
    public async Task RefreshDataAsync()
    {
        TimeValue = 0;
        IsOpen = true;

        SiteModels.Clear();

        var sites = await webDb.SiteModels.ToListAsync();

        // 清除 sites 中所有的描述
        sites.ForEach(site => site.Description = string.Empty);
        // 创建所有的 UpdateStatus 任务
        var updateTasks = sites.Select(site => site.UpdateStatus()).ToList();


        // 并发执行所有 UpdateStatus 任务
        await Task.WhenAll(updateTasks);

        // 更新 UI
        foreach (var site in sites)
        {
            SiteModels.Add(site);
        }

        await webDb.SaveChangesAsync();

        IsOpen = false;
    }
    private DelegateCommand _refreshDataAsyncCommand;
    /// <summary>
    /// 刷新数据命令
    /// </summary>
    public DelegateCommand RefreshDataAsyncCommand => _refreshDataAsyncCommand ??= new DelegateCommand(async () => await RefreshDataAsync());


}
