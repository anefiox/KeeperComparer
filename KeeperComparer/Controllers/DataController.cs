using KeeperComparer.Models;
using KeeperComparer.Services;
using Microsoft.AspNetCore.Mvc;

namespace KeeperComparer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly RecordRepository _recordRepository;
        private readonly ILogger<DataController> _logger; 

        public DataController(RecordRepository recordRepository, ILogger<DataController> logger)
        {
            _recordRepository = recordRepository;
            _logger = logger;
            _logger.LogInformation("DataController constructor has been executed."); 
        }

        [HttpGet("recordsA")]
        public async Task<ActionResult<List<KeeperRecord>>> GetRecordsA()
        {
            _logger.LogInformation("--- GET /api/data/recordsA endpoint was called. ---"); 
            try
            {
                var records = await _recordRepository.GetRecordsAAsync();
                _logger.LogInformation($"Successfully found {records.Count} records in RecordsA.json");
                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting records from RecordsA.json");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("recordsB")]
        public async Task<ActionResult<List<KeeperRecord>>> GetRecordsB()
        {
            _logger.LogInformation("--- GET /api/data/recordsB endpoint was called. ---"); 
            var records = await _recordRepository.GetRecordsBAsync();
            return Ok(records);
        }
    }
}