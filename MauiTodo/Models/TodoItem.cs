namespace MauiTodo.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime Due { get; set; }
    public bool Done { get; set; } = false;
}
