using KeeperComparer.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeeperComparer.Services
{
    public class RecordRepository
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RecordRepository(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<List<KeeperRecord>> GetRecordsAAsync()
        {
            return await ReadRecordsFromFile("RecordsA.json");
        }

        public async Task<List<KeeperRecord>> GetRecordsBAsync()
        {
            return await ReadRecordsFromFile("RecordsB.json");
        }

        private async Task<List<KeeperRecord>> ReadRecordsFromFile(string fileName)
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Data", fileName);

            if (!File.Exists(filePath))
            {
                return new List<KeeperRecord>();
            }

            var jsonString = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return new List<KeeperRecord>();
            }

            var records = JsonSerializer.Deserialize<List<KeeperRecord>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return records ?? new List<KeeperRecord>();
        }
    }
}