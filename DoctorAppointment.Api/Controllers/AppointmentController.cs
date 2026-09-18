using DoctorAppointment.Api.Data;
using DoctorAppointment.Api.DTOs;
using DoctorAppointment.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AppointmentController : ControllerBase
{
    [HttpPost("book")]
    public IActionResult Book(BookAppointmentDto data)
    {
        using ClinicDbContext context = new();

        bool validDateTime = DateTime.TryParse(
            data.AppointmentDate + " " + data.AppointmentTime,
            out DateTime appointmentDateTime);

        if (!validDateTime)
            return BadRequest(
                new { message = "Invalid date or time" });

        if (appointmentDateTime < DateTime.Now)
            return BadRequest(
                new { message = "Past time not allowed" });

        if (appointmentDateTime.Hour < 9 ||
            appointmentDateTime.Hour > 21 ||
            appointmentDateTime.Minute % 15 != 0 ||
            appointmentDateTime.Second != 0 ||
            (appointmentDateTime.Hour == 21 &&
             appointmentDateTime.Minute > 0))
        {
            return BadRequest(
                new { message = "Select time between 9 AM and 9 PM" });
        }

        bool booked = context.Appointments.Any(x =>
            x.AppointmentDateTime == appointmentDateTime);

        if (booked)
            return BadRequest(
                new { message = "Time already booked" });

        Appointment appointment = new()
        {
            PatientName = data.PatientName,
            PhoneNumber = data.PhoneNumber,
            Description = data.Description,
            AppointmentDateTime = appointmentDateTime,
            Status = "Pending"
        };

        context.Appointments.Add(appointment);
        context.SaveChanges();

        return Ok(
            new { message = "Appointment booked" });
    }

    [HttpGet]
    public IActionResult GetAppointments(DateTime date)
    {
        using ClinicDbContext context = new();

        DateTime nextDate = date.Date.AddDays(1);

        var appointments = context.Appointments
            .Where(x =>
                x.AppointmentDateTime >= date.Date &&
                x.AppointmentDateTime < nextDate)
            .OrderBy(x => x.AppointmentDateTime)
            .ToList();

        return Ok(appointments);
    }

    [HttpPut("{id}/status")]
    public IActionResult UpdateStatus(
        int id,
        AppointmentStatusDto data)
    {
        using ClinicDbContext context = new();

        Appointment? appointment =
            context.Appointments.Find(id);

        if (appointment == null)
            return NotFound();

        if (data.Status != "Appeared" &&
            data.Status != "NoShow")
        {
            return BadRequest(
                new { message = "Invalid status" });
        }

        appointment.Status = data.Status;

        context.SaveChanges();

        return Ok(
            new { message = "Status updated" });
    }
}