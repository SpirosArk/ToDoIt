using ToDoIt.Endpoints;

var app = WebApplication.Create(args);

app.MapToDoEndpoints();
await app.RunAsync();