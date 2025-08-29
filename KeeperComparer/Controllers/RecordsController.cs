using KeeperComparer.Models;
using KeeperComparer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace KeeperComparer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordsController : ControllerBase
    {
        private readonly RecordRepository _recordRepository;
        private readonly RecordComparisonService _comparisonService;

        public RecordsController(RecordRepository recordRepository, RecordComparisonService comparisonService)
        {
            _recordRepository = recordRepository;
            _comparisonService = comparisonService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRecordById(int id)
        {
            var recordsA = await _recordRepository.GetRecordsAAsync();
            var recordsB = await _recordRepository.GetRecordsBAsync();
            var recordA = recordsA.FirstOrDefault(r => r.Id == id);
            var recordB = recordsB.FirstOrDefault(r => r.Id == id);
            if (recordA == null && recordB == null) return NotFound();
            return Ok(new { recordA, recordB });
        }

        [HttpGet]
        public async Task<ActionResult> GetPagedRecords([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var recordsA = await _recordRepository.GetRecordsAAsync();
            var recordsB = await _recordRepository.GetRecordsBAsync();

            var allDataPairs = recordsA.Select(recordA =>
            {
                var recordB = recordsB.FirstOrDefault(r => r.Id == recordA.Id);
                var comparisonResult = _comparisonService.CompareRecords(recordA, recordB ?? new KeeperRecord { Id = recordA.Id });

                return new { OriginalRecord = recordA, Result = comparisonResult };
            }).ToList();

            var summary = new
            {
                StrongMatches = allDataPairs.Count(p => p.Result.OverallResult == Models.DTOs.MatchStrength.Strong || p.Result.OverallResult == Models.DTOs.MatchStrength.Exact),
                PartialMatches = allDataPairs.Count(p => p.Result.OverallResult == Models.DTOs.MatchStrength.Partial),
                WeakMatches = allDataPairs.Count(p => p.Result.OverallResult == Models.DTOs.MatchStrength.Weak),
                Mismatches = allDataPairs.Count(p => p.Result.OverallResult == Models.DTOs.MatchStrength.NoMatch)
            };

            var pagedPairs = allDataPairs
                .OrderBy(p => p.OriginalRecord.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pageData = pagedPairs.Select(pair => new
            {
                Record = pair.OriginalRecord,
                OverallStatus = pair.Result.OverallResult,
                FieldStatuses = pair.Result.FieldResults.ToDictionary(fr => fr.FieldName.Replace(" ", ""), fr => fr.Status)
            }).ToList();

            return Ok(new
            {
                Pagination = new
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalRecords = recordsA.Count,
                    TotalPages = (int)System.Math.Ceiling(recordsA.Count / (double)pageSize)
                },
                Summary = summary,
                Records = pageData
            });
        }
    }
}