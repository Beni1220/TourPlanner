// DAL/TourPlannerContext.cs
using Microsoft.EntityFrameworkCore;

public class TourPlannerContext : DbContext
{
    public TourPlannerContext(DbContextOptions<TourPlannerContext> options) 
        : base(options) { }

    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourLogs> TourLogs { get; set; }
    public DbSet<TourCoordinate> TourCoordinates { get; set; }
    public DbSet<User> Users { get; set; }
    
}