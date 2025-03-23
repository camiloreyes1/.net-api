using Microsoft.AspNetCore.Authentication.Cookies; // Imports namespace for cookie-based authentication (not used in this example)
using Microsoft.AspNetCore.Http.HttpResults; // Imports types for strongly-typed HTTP responses

// Create a builder for configuring the web application
var builder = WebApplication.CreateBuilder(args);

// Build the web application
var app = builder.Build();

// Initialize an in-memory list to store todo items
var todos = new List<Todo>();

// Route to get all todos
// Returns the list of todos
app.MapGet("/todos", () => todos);

// Route to get a specific todo by its ID
// Returns a 200 OK response with the todo if found, otherwise a 404 Not Found response
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) => 
{
    // Find the todo with the specified ID
    var targetTodo = todos.SingleOrDefault(t => id == t.Id);
    return targetTodo is null
            ? TypedResults.NotFound() // Return 404 if not found
            : TypedResults.Ok(targetTodo); // Return 200 with the todo if found
});

// Route to create a new todo
// Accepts a Todo object in the request body, adds it to the list, and returns a 201 Created response
app.MapPost("/todos", (Todo task) => 
{
    // Add the new todo to the list
    todos.Add(task);
    return TypedResults.Created("/todos/{id}", task); // Return 201 with the location of the new resource
});

// Route to delete a todo by its ID
// Removes the todo with the specified ID from the list and returns a 204 No Content response
app.MapDelete("/todos/{id}", (int id) => 
{
    // Remove the todo with the specified ID
    todos.RemoveAll(t => id == t.Id);
    return TypedResults.NoContent(); // Return 204 No Content
});

// Run the application
app.Run();

// Define a record type for representing todos
// Contains an ID, Name, DueDate, and IsCompleted flag
public record Todo(int Id, string Name, DateTime DueDate, bool IsCompleted)
{
    
}
