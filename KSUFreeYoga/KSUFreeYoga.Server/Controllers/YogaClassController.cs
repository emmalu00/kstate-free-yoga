using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KSUFreeYoga.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YogaClassController : ControllerBase
    {
        private readonly ILogger<YogaClassController> _logger;

        public YogaClassController(ILogger<YogaClassController> logger)
        {
            _logger = logger;
        }

        /*[HttpGet(Name = "GetYogaClassInformation")]
        public async Task<IEnumerable<YogaClass>> Get()
        {
            return await _context.YogaClasses.ToListAsync();
        }*/
    }
}
