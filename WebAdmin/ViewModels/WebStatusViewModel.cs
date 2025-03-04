using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WebAdmin.Db;
using WebAdmin.Models;

namespace WebAdmin.ViewModels;

public class WebStatusViewModel : BindableBase
{
    private readonly WebDb webDb;
    public ObservableCollection<SiteModel> SiteModels { get; set; }
    public WebStatusViewModel(WebDb webDb)
    {
        this.webDb = webDb;
        InitDb();
    }

    private void InitDb()
    {
        webDb.SiteModels.ToList().ForEach(x => SiteModels.Add(x));
    }
}
