using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdmin.Models;
using WebAdmin.Units;

namespace WebAdmin.Db;

public class WebDb : DbContext
{
    public DbSet<SiteModel> SiteModels { get; set; }

    public string DbPath { get; set; }


    public WebDb()
    {

    }

    public WebDb(DbContextOptions<WebDb> options) : base(options)
    {
        Database.MigrateAsync();
    }

    public WebDb(string path)
    {
        DbPath = path;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);//一定要写
        optionsBuilder.UseLazyLoadingProxies(); //启用延迟加载

        // 只有在迁移时使用 SQLite
        DbPath = Tools.GetAndCreatDbFilePath();
        optionsBuilder.UseSqlite($"Data Source={DbPath}");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
