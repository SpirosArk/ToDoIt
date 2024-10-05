using ToDoIt.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoIt.ApplicationDbContext
{
    public class TodoDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Todos");
        }
    }
}
