using DemoAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    // Визначення таблиці в БД
    public DbSet<StudentEntity> StudentRegister { get; set; }
}