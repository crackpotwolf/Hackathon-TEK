using Hackathon_TEK.Models;
using Microsoft.EntityFrameworkCore;

namespace Hackathon_TEK
{
    public class HackathonContext : DbContext
    {
        public virtual DbSet<Test> Tests { get; set; }

        public virtual DbSet<Region> Regions { get; set; }

        public virtual DbSet<Reason> Reasons { get; set; }

        public virtual DbSet<Fire> Fires { get; set; }

        public virtual DbSet<Earthquake> Earthquakes { get; set; }

        public virtual DbSet<Weather> Weather { get; set; }

        public HackathonContext()
        { }

        public HackathonContext(DbContextOptions<HackathonContext> options) : base(options)
        {

        }
    }
}