using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDosProject.Domain.Entities;

namespace ToDosProject.Infraestructure.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> opt) : IdentityDbContext<User>(opt)
    {
        public DbSet<ToDo> ToDo { get; set; }
    }
}
