using MauiTodo.Data;
using MauiTodo.Models;
using System.Collections.ObjectModel;

namespace MauiTodo
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<TodoItem> Todos { get; set; } = [];

        private readonly Database database;

        public MainPage()
        {
            InitializeComponent();
            database = new Database();
            _ = Initialize();
        }

        private async Task Initialize()
        {
            List<TodoItem> todos = await database.GetTodos();
            foreach (TodoItem todo in todos)
            {
                Todos.Add(todo);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            TodoItem todo = new()
            {
                Due = DueDatepicker.Date,
                Title = TodoTitleEntry.Text
            };

            int inserted = await database.AddTodo(todo);

            if (inserted != 0)
            {
                Todos.Add(todo);

                TodoTitleEntry.Text = string.Empty;
                DueDatepicker.Date = DateTime.Now;
            }
        }

        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            await Shell.Current.DisplayAlert(item?.Text, $"You invoked the {item?.Text} action.", "OK");
        }
    }
}
