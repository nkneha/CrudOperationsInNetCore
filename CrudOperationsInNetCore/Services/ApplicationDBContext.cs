using CrudOperationsInNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudOperationsInNetCore.Services
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options) 
        {

        }
        public DbSet<Product> Products { get; set; }


    }
}
