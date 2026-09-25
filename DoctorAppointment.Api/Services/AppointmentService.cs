
using DoctorAppointment.Api.Data;
using DoctorAppointment.Api.DTOs;
using DoctorAppointment.Api.Models;

namespace DoctorAppointment.Api.Services;

public sealed class AppointmentService
{
    public string Book(BookAppointmentDto data)
    {
        using ClinicDbContext context = new();

        bool valid = DateTime.TryParse(
            data.AppointmentDate + " " + data.AppointmentTime,
            out DateTime dateTime);

        if (!valid)
            return "Invalid date or time";

        if (dateTime < DateTime.Now)
            return "Past time not allowed";

        if (dateTime.Hour < 9 ||
            dateTime.Hour > 21 ||
            dateTime.Minute % 15 != 0 ||
            dateTime.Second != 0 ||
            (dateTime.Hour == 21 && dateTime.Minute > 0))
        {
            return "Select time between 9 AM and 9 PM";
        }

        bool booked = context.Appointments.Any(x =>
            x.AppointmentDateTime == dateTime);

        if (booked)
            return "Time already booked";

        Appointment appointment = new()
        {
            PatientName = data.PatientName,
            PhoneNumber = data.PhoneNumber,
            Description = data.Description,
            AppointmentDateTime = dateTime,
            Status = "Pending"
        };

        context.Appointments.Add(appointment);
        context.SaveChanges();

        return "Appointment booked";
    }

    public List<Appointment> GetAppointments(DateTime date)
    {
        using ClinicDbContext context = new();

        DateTime nextDate = date.Date.AddDays(1);

        return context.Appointments
            .Where(x =>
                x.AppointmentDateTime >= date.Date &&
                x.AppointmentDateTime < nextDate)
            .OrderBy(x => x.AppointmentDateTime)
            .ToList();
    }

    public string UpdateStatus(
        int id, AppointmentStatusDto data)
    {
        using ClinicDbContext context = new();

        Appointment? appointment =
            context.Appointments.Find(id);

        if (appointment == null)
            return "Appointment not found";

        if (data.Status != "Appeared" &&
            data.Status != "NoShow")
        {
            return "Invalid status";
        }

        appointment.Status = data.Status;
        context.SaveChanges();

        return "Status updated";
    }
}
