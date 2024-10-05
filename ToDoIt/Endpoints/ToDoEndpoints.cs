using ToDoIt.Models;
using ToDoIt.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace ToDoIt.Endpoints
{
    public static class ToDoEndpoints
    {
        public static void MapToDoEndpoints(this WebApplication app) 
        {
            app.MapGet("/api/todos", GetTodos);
            app.MapGet("/api/todos/{id}", GetTodo);
            app.MapPost("/api/todos", CreateTodo);
            app.MapPost("/api/todos/{id}", UpdateCompleted);
            app.MapDelete("/api/todos/{id}", DeleteTodo);



            async Task GetTodos(HttpContext context)
            {
                using var db = new TodoDbContext();
                var todos = await db.Todos.ToListAsync();

                await context.Response.WriteAsJsonAsync(todos);
            }

            async Task GetTodo(HttpContext context)
            {
                if (!context.Request.RouteValues.TryGetValue("id", out var idValue) || !int.TryParse(idValue?.ToString(), out int id))
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                using var db = new TodoDbContext();
                var todo = await db.Todos.FindAsync(id);
                if (todo is null)
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                await context.Response.WriteAsJsonAsync(todo);
            }

            async Task CreateTodo(HttpContext context)
            {
                var todo = await context.Request.ReadFromJsonAsync<Todo>();

                using var db = new TodoDbContext();
                await db.Todos.AddAsync(todo);
                await db.SaveChangesAsync();

                context.Response.StatusCode = 204;
            }

            async Task UpdateCompleted(HttpContext context)
            {
                if (!context.Request.RouteValues.TryGetValue("id", out var idValue) || !int.TryParse(idValue?.ToString(), out int id))
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                using var db = new TodoDbContext();
                var todo = await db.Todos.FindAsync(id);

                if (todo is null)
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                todo.IsComplete = true;
                await db.SaveChangesAsync();

                context.Response.StatusCode = 204;
            }

            async Task DeleteTodo(HttpContext context)
            {
                if (!context.Request.RouteValues.TryGetValue("id", out var idValue) || !int.TryParse(idValue?.ToString(), out int id))
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                using var db = new TodoDbContext();
                var todo = await db.Todos.FindAsync(id);
                if (todo is null)
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                db.Todos.Remove(todo);
                await db.SaveChangesAsync();

                context.Response.StatusCode = 204;
            }

        }
    }
}
