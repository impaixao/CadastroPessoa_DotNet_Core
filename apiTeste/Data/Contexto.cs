using Microsoft.EntityFrameworkCore;
using apiTeste.Models;

namespace apiTeste.Data
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
            
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}