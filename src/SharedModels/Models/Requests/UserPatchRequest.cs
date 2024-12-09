using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.Requests;
public class UserPatchRequest
{
    public string? Username { get; set; }
    public string? Street { get; set; }
    public string? StreetPlus { get; set; }
    public string? City { get; set; } = "";
    public string? State { get; set; } = "";
    public string? Zip { get; set; } = "";
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
}
