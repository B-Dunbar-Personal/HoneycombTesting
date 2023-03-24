using Demo.Api.Database;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly IDemoDatabase _demoDatabase;

        public DemoController(IDemoDatabase demoDatabase)
        {
            _demoDatabase = demoDatabase;
        }

        [HttpGet]
        [Route("Address")]
        public async Task<IActionResult> GetAddress(int id)
        {
            var person = (await _demoDatabase.GetIndividualsAddressLines(id)).ToList();
            switch (person.Count())
            {
                case 1:
                    var record = person[0];
                    if (string.IsNullOrEmpty(record.Address2))
                        return Ok($"{record.Address1} | {record.City.ToUpper()}");

                    return Ok($"{record.Address1} | {record.Address2} | {record.City.ToUpper()}");

                case > 1:
                    return StatusCode(500, "More than one record found, somethings wrong");

                default:
                    return NotFound("No Records Found");
            }


        }
    }
}