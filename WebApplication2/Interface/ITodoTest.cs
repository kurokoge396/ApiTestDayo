namespace WebApplication2.Interface
{
    public interface ITodoTest
    {
        public TodoTest GetTodo();
        public TodoTest AddTodo(TodoTest item);
        public bool DeleteTodo(int id);
    }

    public class TodoTestImpl : ITodoTest
    {
        public List<TodoTest> _items;

        public TodoTestImpl()
        {
            _items = new List<TodoTest>();
        }

        public TodoTest GetTodo()
        {
            return new TodoTest { Id = 1, Title = "Sample Todo", IsCompleted = false };
        }
        public TodoTest AddTodo(TodoTest item)
        {
            _items.Add(item);
            return item;
        }
        public bool DeleteTodo(int id)
        {
            var item = _items.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                _items.Remove(item);
                return true;
            }
            return false;
        }
    }
}
