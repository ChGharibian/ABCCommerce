using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;
[Table("Users")]
public class User
{
    [Key] public int ID { get; set; }
    public string Name { get; set; } = "";
}
