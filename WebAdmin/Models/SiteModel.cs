using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebAdmin.Units;

namespace WebAdmin.Models;

public class SiteModel : BindableBase
{
    private long _id;
    private string _address;
    private string _name;
    private string _description;
    private DateTime _lastScanTime;
    private bool _status;

    [Key]
    [Display(Name = "Site ID")]
    [Column("任务Id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id
    {
        get { return _id; }
        set { SetProperty(ref _id, value); }
    }

    [Column("网址")]
    public string Address
    {
        get { return _address; }
        set { SetProperty(ref _address, value); }
    }

    [Column("姓名")]
    public string Name
    {
        get { return _name; }
        set { SetProperty(ref _name, value); }
    }

    [Column("描述")]
    public string Description
    {
        get { return _description; }
        set { SetProperty(ref _description, value); }
    }

    [Display(Name = "Scan Interval")]
    [Column("最后扫描时间")]
    public DateTime LastScanTime
    {
        get { return _lastScanTime; }
        set { SetProperty(ref _lastScanTime, value); }
    }

    [Column("状态")]
    [NotMapped]
    public bool Status
    {
        get { return _status; }
        set { SetProperty(ref _status, value); }
    }

    async internal Task UpdateStatus()
    {
        if (Address != null)
        {
            try
            {
                Status = await Tools.IsWebsiteUp(Address);
            }
            catch (Exception ex)
            {
                Description = ex.Message;
            }
            LastScanTime = DateTime.Now;
        }
        else
        {
            Description = "空网址";
        }
    }
}
