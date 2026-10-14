using DoctorAppointment.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Api.Data;

public sealed class ClinicDbContext : DbContext
{
    public ClinicDbContext(
        DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public DbSet<Appointment> Appointments { get; set; } = null!;
}
