namespace DoctorAppointment.Api.DTOs;

public sealed class BookAppointmentDto
{
    public string PatientName { get; set; } = string.Empty;

    public long PhoneNumber { get; set; }

    public string? Description { get; set; }

    public string AppointmentDate { get; set; } = string.Empty;

    public string AppointmentTime { get; set; } = string.Empty;
}