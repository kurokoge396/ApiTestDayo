using Microsoft.AspNetCore.Mvc;
using WebApplication2.Interface;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        public ITodoTest _todoTest;

        public TestController(ITodoTest todoTest)
        {
            _todoTest = todoTest;
        }

        [HttpGet]
        public string Get()
        {
            return "Get_WebAPIだな";
        }

        [HttpGet]
        [Route("todo")]
        public string GetTodo()
        {
            return "Get_TODOだな";
        }

        [HttpPost]
        public ActionResult<TodoTest> AddTodo([FromBody] TodoTest item)
        {
            var todo = _todoTest.AddTodo(item);
            return todo is null ? BadRequest() : CreatedAtAction(nameof(GetTodo), new { Id = todo.Id }, todo);
        }
    }
}
