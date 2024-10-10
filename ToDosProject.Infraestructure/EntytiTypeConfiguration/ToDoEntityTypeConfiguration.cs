using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDosProject.Domain.Entities;
using ToDosProject.Domain.Services;

namespace ToDosProject.Infraestructure.EntytiTypeConfiguration
{
    public class ToDoEntityTypeConfiguration(UserContextService userContextService) : IEntityTypeConfiguration<ToDo>
    {
        public void Configure(EntityTypeBuilder<ToDo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsConcluded);
            builder.Property(x => x.Title)
                   .IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId);

            builder.HasQueryFilter(x => x.UserId == userContextService.GetUserId());
        }

    }
}
