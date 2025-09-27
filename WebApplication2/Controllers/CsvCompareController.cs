using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CsvCompareController : ControllerBase
    {
        [HttpGet(Name = "GetCsvCompare")]
        public void Get()
        {

        }

        [HttpPost(Name = "PostCsvCompare")]
        public void Post()
        {

        }
    }


}
