using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Elasticsearch;
namespace FluentdTester
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly Logger logger;
        public LogController() {
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", "LogTester")
                .WriteTo.File(new ElasticsearchJsonFormatter(), "logs/fluentd-test.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        // GET: api/log/test
        [HttpGet("test")]
        public IActionResult Get(){
            logger.Debug(string.Format("This is sample log."));
            return Ok();
        }
    }
}
