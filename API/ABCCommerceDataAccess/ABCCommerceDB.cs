using ABCCommerceDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess;
public class ABCCommerceDB : DbContext
{
    public ABCCommerceDB()
    {
    }

    public ABCCommerceDB(DbContextOptions<ABCCommerceDB> options)
        : base(options)
    {
    }
    DbSet<User> Users { get; set; }
}
