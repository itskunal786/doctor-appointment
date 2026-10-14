using DoctorAppointment.Api.Data;
using DoctorAppointment.Api.DTOs;
using DoctorAppointment.Api.Models;

namespace DoctorAppointment.Api.Services;

public sealed class AppointmentService
{
    private readonly ClinicDbContext context;

    public AppointmentService(ClinicDbContext context)
    {
        this.context = context;
    }

    public string Book(BookAppointmentDto data)
    {
        bool validDateTime = DateTime.TryParse(
            data.AppointmentDate + " " + data.AppointmentTime,
            out DateTime appointmentDateTime);

        if (!validDateTime)
            return "Invalid date or time";

        if (appointmentDateTime < DateTime.Now)
            return "Past time not allowed";

        if (appointmentDateTime.Hour < 9 ||
            appointmentDateTime.Hour > 21 ||
            appointmentDateTime.Minute % 15 != 0 ||
            appointmentDateTime.Second != 0 ||
            (appointmentDateTime.Hour == 21 &&
             appointmentDateTime.Minute > 0))
        {
            return "Select time between 9 AM and 9 PM";
        }

        bool booked = context.Appointments.Any(x =>
            x.AppointmentDateTime == appointmentDateTime);

        if (booked)
            return "Time already booked";

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

        return "Appointment booked";
    }

    public List<Appointment> GetAppointments(DateTime date)
    {
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
