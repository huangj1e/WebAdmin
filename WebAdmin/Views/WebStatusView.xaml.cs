using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WebAdmin.Models;
using WebAdmin.ViewModels;

namespace WebAdmin.Views;

/// <summary>
/// Interaction logic for WebStatusView
/// </summary>
public partial class WebStatusView : UserControl
{
    public WebStatusView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 表格行编辑完成
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
        if(e.Row.Item is SiteModel site)
        {
            var context = ((WebStatusViewModel)DataContext).webDb;
            if (context.SiteModels.Any(p => p.Id == site.Id))
            {
                context.Entry(site).State = EntityState.Modified;
            }
            else
            {
                context.SiteModels.Add(site);
            }
            context.SaveChanges();
        } 
    }

    /// <summary>
    /// 表格加载完成
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dataGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        // 表格加载完成后自动滑到最底部
        if (dataGrid.Items.Count > 0)
        {
            dataGrid.SelectedItem = dataGrid.Items[dataGrid.Items.Count - 1];
            dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }
    }

}
