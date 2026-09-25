using DoctorAppointment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ReportController : ControllerBase
{
    private readonly ReportService service = new();

    [HttpGet]
    public IActionResult GetReport(
        DateTime fromDate, DateTime toDate)
    {
        if (fromDate.Date > toDate.Date)
            return BadRequest(
                new { message = "Invalid date range" });

        return Ok(service.GetReport(fromDate, toDate));
    }
}
