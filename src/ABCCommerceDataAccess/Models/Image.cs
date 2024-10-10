using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;
public class Image
{
    [Key]
    [StringLength(100)]
    public string Key { get; set; } = "";
    public string Base64 { get; set; } = "";
    [StringLength(30)]
    public string Type { get; set; } = "";
}
