using Microsoft.EntityFrameworkCore;
using ToDosProject.Domain;
using ToDosProject.Infraestructure;

namespace ToDosProject.ApiService.MapGroups
{
    public class ToDoItemEndnpoits
    {
        public static async Task<IResult> GetAll(AppDbContext db)
        {
            return TypedResults.Ok(await db.ToDo.ToArrayAsync());
        }

        public static async Task<IResult> GetById(int id, AppDbContext db)
            => await db.ToDo.FindAsync(id)
                is ToDo ToDo
                    ? TypedResults.Ok(ToDo)
                    : TypedResults.NotFound();

        public static async Task<IResult> Create(ToDo ToDo, AppDbContext db)
        {
            db.ToDo.Add(ToDo);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/ToDoitems/{ToDo.Id}", ToDo);
        }

        public static async Task<IResult> Update(int id, ToDo inputToDo, AppDbContext db)
        {
            var ToDo = await db.ToDo.FindAsync(id);

            if (ToDo is null) return TypedResults.NotFound();

            ToDo.Title = inputToDo.Title;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<IResult> Delete(int id, AppDbContext db)
        {
            if (await db.ToDo.FindAsync(id) is not ToDo ToDo) 
                return TypedResults.NotFound();

            db.ToDo.Remove(ToDo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        public static async Task<IResult> Conclude(int id, AppDbContext db)
        {
            if (await db.ToDo.FindAsync(id) is not ToDo toDo)
                return TypedResults.NotFound();

            toDo.Conclude();
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        public static async Task<IResult> Unconclude(int id, AppDbContext db)
        {
            if (await db.ToDo.FindAsync(id) is not ToDo toDo)
                return TypedResults.NotFound();

            toDo.Unconclude();
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
    }
}
