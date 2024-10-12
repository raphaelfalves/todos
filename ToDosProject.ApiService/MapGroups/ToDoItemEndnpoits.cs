using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ToDosProject.Domain.Entities;
using ToDosProject.Domain.Exceptions;
using ToDosProject.Infraestructure.Context;

namespace ToDosProject.ApiService.MapGroups
{
    public class ToDoItemEndnpoits()
    {
        public static async Task<string> GetUserId(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            var user = await db.User.FirstOrDefaultAsync(u => u.Email == httpContextAccessor.HttpContext!.User.Identity!.Name)
                ?? throw new UserNotFoundExpception("Usuário não econtrado!");

            return user.Id;
        }

        public static async Task<IResult> GetAll(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            string userid = await GetUserId(db, httpContextAccessor);
            return TypedResults.Ok(await db.ToDo.Where(u => u.UserId == userid).ToArrayAsync());
        }

        public static async Task<IResult> GetById(int id, AppDbContext db)
            => await db.ToDo.FindAsync(id)
                is ToDo ToDo
                    ? TypedResults.Ok(ToDo)
                    : TypedResults.NotFound();

        public static async Task<IResult> Create(ToDo ToDo, AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                ToDo.UserId ??= await GetUserId(db, httpContextAccessor);

                db.ToDo.Add(ToDo);
                await db.SaveChangesAsync();

                return TypedResults.Created($"/ToDoitems/{ToDo.Id}", ToDo);
            }
            catch (UserNotFoundExpception)
            {
                return TypedResults.Unauthorized();
            }
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
