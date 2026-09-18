using DoctorAppointment.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ReportController : ControllerBase
{
    [HttpGet]
    public IActionResult GetReport(
        DateTime fromDate,
        DateTime toDate)
    {
        if (fromDate.Date > toDate.Date)
            return BadRequest(
                new { message = "Invalid date range" });

        using ClinicDbContext context = new();

        DateTime endDate =
            toDate.Date.AddDays(1);

        var appointments = context.Appointments
            .Where(x =>
                x.AppointmentDateTime >= fromDate.Date &&
                x.AppointmentDateTime < endDate);

        return Ok(new
        {
            appeared = appointments.Count(
                x => x.Status == "Appeared"),

            noShow = appointments.Count(
                x => x.Status == "NoShow")
        });
    }
}