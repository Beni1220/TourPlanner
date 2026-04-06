// DAL/TourPlannerContext.cs
using Microsoft.EntityFrameworkCore;

public class TourPlannerContext : DbContext
{
    public TourPlannerContext(DbContextOptions<TourPlannerContext> options) 
        : base(options) { }

    public DbSet<Tour> Tours { get; set; }
    
}