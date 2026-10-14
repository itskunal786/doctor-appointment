using DoctorAppointment.Api.Data;

namespace DoctorAppointment.Api.Services;

public sealed class ReportService
{
    private readonly ClinicDbContext context;

    public ReportService(ClinicDbContext context)
    {
        this.context = context;
    }

    public object GetReport(DateTime fromDate, DateTime toDate)
    {
        DateTime endDate = toDate.Date.AddDays(1);

        var appointments = context.Appointments
            .Where(x =>
                x.AppointmentDateTime >= fromDate.Date &&
                x.AppointmentDateTime < endDate);

        return new
        {
            appeared = appointments.Count(
                x => x.Status == "Appeared"),

            noShow = appointments.Count(
                x => x.Status == "NoShow")
        };
    }
}
