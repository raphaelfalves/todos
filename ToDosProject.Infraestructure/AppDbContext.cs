using Microsoft.EntityFrameworkCore;
using ToDosProject.Domain;

namespace ToDosProject.Infraestructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
    {
        public DbSet<ToDo> ToDo { get; set; }
    }
}
