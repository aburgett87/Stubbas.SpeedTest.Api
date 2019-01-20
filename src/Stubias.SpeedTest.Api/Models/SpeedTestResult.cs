using System;
using System.ComponentModel.DataAnnotations;

namespace Stubias.SpeedTest.Api.Models
{
    public class SpeedTestResult
    {
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime ExecutionDateTime { get; set; }
        [Required]
        public decimal AverageDownloadSpeed { get; set; }
        [Required]
        public decimal MaximumDownloadSpeed { get; set; }
        [Required]
        public decimal AverageUploadSpeed { get; set; }
        [Required]
        public decimal MaximumUploadSpeed { get; set; }
        [Required]
        public decimal Latency { get; set; }
        [Required]
        public string TestServerName { get; set; }
        [Required]
        public string NodeName { get; set; }
    }
}