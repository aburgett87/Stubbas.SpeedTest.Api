using System;
using Microsoft.AspNetCore.Mvc;

namespace Stubias.SpeedTest.Api.Models.Input
{
    public class SpeedTestResultInputModel
    {
        [FromRoute(Name = "location")]
        public string Location { get; set; }
        [FromQuery]
        public DateTime StartDateTime { get; set; }
        [FromQuery]
        public DateTime EndDateTime { get; set; }
    }
}
