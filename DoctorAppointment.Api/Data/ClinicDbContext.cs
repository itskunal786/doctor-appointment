using DoctorAppointment.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Api.Data;

public sealed class ClinicDbContext : DbContext
{
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=DESKTOP-AVTPTVK;Database=DoctorAppointment;Trusted_Connection=True;TrustServerCertificate=True;");

        base.OnConfiguring(optionsBuilder);
    }
}