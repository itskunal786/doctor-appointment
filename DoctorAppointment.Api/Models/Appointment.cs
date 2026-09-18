namespace DoctorAppointment.Api.Models;

public sealed class Appointment
{
    public int AppointmentId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public long PhoneNumber { get; set; }
    public string? Description { get; set; } 
    public DateTime AppointmentDateTime { get; set; }
    public string Status { get; set; } = "Pending";
}