using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAdmin.Models;

public class SiteModel
{
    [Key]
    [Display(Name = "Site ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Address { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    [Display(Name = "Scan Interval")]
    public DateTime LastScanTime { get; set; }

    public bool Status { get; set; }
}
