using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Controls;
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
}
