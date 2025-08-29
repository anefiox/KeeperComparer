using KeeperComparer.Models;
using KeeperComparer.Models.DTOs;
using KeeperComparer.Services;
using KeeperComparer.Validators;
using Microsoft.AspNetCore.Mvc;

namespace KeeperComparer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComparisonController : ControllerBase
    {
        private readonly RecordComparisonService _comparisonService;
        private readonly KeeperRecordValidator _recordValidator;

        public ComparisonController(RecordComparisonService comparisonService, KeeperRecordValidator recordValidator)
        {
            _comparisonService = comparisonService;
            _recordValidator = recordValidator;
        }

        [HttpPost("compare")]
        public ActionResult<ComparisonResult> Compare([FromBody] KeeperRecord[] records)
        {
            Console.WriteLine($"Received {records?.Length ?? 0} records for comparison.");
            if (records != null && records.Length > 0)
            {
                Console.WriteLine($"Record A is null: {records[0] == null}");
                Console.WriteLine($"Record B is null: {records[1] == null}");
            }

            if (records == null || records.Length != 2)
            {
                return BadRequest("Please provide exactly two records to compare.");
            }

            var recordA = records[0];
            var recordB = records[1];

            var (isAValid, errorsA) = _recordValidator.Validate(recordA);
            if (!isAValid) return BadRequest(new { recordA_errors = errorsA });

            var (isBValid, errorsB) = _recordValidator.Validate(recordB);
            if (!isBValid) return BadRequest(new { recordB_errors = errorsB });

            var result = _comparisonService.CompareRecords(recordA, recordB);

            return Ok(result);
        }
    }
}