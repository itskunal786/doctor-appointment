using DoctorAppointment.Api.DTOs;
using DoctorAppointment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AppointmentController : ControllerBase
{
    private readonly AppointmentService service;

    public AppointmentController(AppointmentService service)
    {
        this.service = service;
    }

    [HttpPost("book")]
    public IActionResult Book(BookAppointmentDto data)
    {
        string result = service.Book(data);

        if (result != "Appointment booked")
            return BadRequest(new { message = result });

        return Ok(new { message = result });
    }

    [HttpGet]
    public IActionResult GetAppointments(DateTime date)
    {
        return Ok(service.GetAppointments(date));
    }

    [HttpPut("{id}/status")]
    public IActionResult UpdateStatus(
        int id, AppointmentStatusDto data)
    {
        string result = service.UpdateStatus(id, data);

        if (result == "Appointment not found")
            return NotFound();

        if (result != "Status updated")
            return BadRequest(new { message = result });

        return Ok(new { message = result });
    }
}
