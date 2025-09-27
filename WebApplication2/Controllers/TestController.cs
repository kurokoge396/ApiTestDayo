using Microsoft.AspNetCore.Mvc;
using System.Text;
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

        /// <summary>
        /// CSVアップロードAPIとりあえず版
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("csv-multi")]
        [RequestSizeLimit(10 * 1024 * 1024)] // 最大10MB
        public async Task<IActionResult> UploadMultipleCsv(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("ファイルが指定されていません。");

            var resultList = new List<object>();

            foreach (var file in files)
            {
                // 拡張子チェック
                var ext = Path.GetExtension(file.FileName);
                if (!ext.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    return BadRequest($"不正なファイル形式: {file.FileName}");

                var records = new List<string[]>();

                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var columns = line.Split(',');
                        for (int i = 0; i < columns.Length; i++)
                        {
                            columns[i] = SanitizeCell(columns[i]);
                        }
                        records.Add(columns);
                    }
                }

                resultList.Add(new
                {
                    FileName = Path.GetFileName(file.FileName),
                    RowCount = records.Count,
                    Sample = records.Count > 0 ? records[0] : null
                });
            }

            return Ok(resultList);
        }

        private string SanitizeCell(string value)
        {
            if (!string.IsNullOrEmpty(value) &&
                (value.StartsWith("=") || value.StartsWith("+") ||
                 value.StartsWith("-") || value.StartsWith("@")))
            {
                return "'" + value; // CSVインジェクション対策
            }
            return value;
        }
    }
}
