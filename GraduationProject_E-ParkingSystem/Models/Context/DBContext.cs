using Microsoft.EntityFrameworkCore;

namespace GraduationProject_E_ParkingSystem.Models.Context
{
    public class DBContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.; Database=EParkingSystem; Trusted_Connection=true; Encrypt=false;");
            base.OnConfiguring(optionsBuilder);
        }
		public DbSet<User> Users { get; set; }
        public DbSet<ParkingSpots> ParkingSpots { get; set; }
        public DbSet<ParkingSetting> parkingSettings { get; set; }
        public DbSet<ParkingRecord> parkingRecords{ get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }

        //public DbSet<Payment> payments { get; set; }
    }
}


//using Microsoft.EntityFrameworkCore;

//namespace GraduationProject_E_ParkingSystem.Models.Context
//{
//	public class DBContext : DbContext
//	{
//		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//		{
//			optionsBuilder.UseSqlServer("Server=.; Database=EParkingSystem; Trusted_Connection=true; Encrypt=false;");
//			base.OnConfiguring(optionsBuilder);
//		}

//		protected override void OnModelCreating(ModelBuilder modelBuilder)
//		{
//			// Define composite key
//			modelBuilder.Entity<ParkingRecord>()
//	.HasKey(pr => new { pr.UserId, pr.ParkingSpotId });


//			// Define foreign key relationships
//			modelBuilder.Entity<ParkingRecord>()
//				.HasOne(pr => pr.User)
//				.WithMany(u => u.ParkingRecords)
//				.HasForeignKey(pr => pr.UserId)
//				.OnDelete(DeleteBehavior.Restrict);

//			modelBuilder.Entity<ParkingRecord>()
//				.HasOne(pr => pr.ParkingSpot)
//				.WithMany(ps => ps.ParkingRecords)
//				.HasForeignKey(pr => pr.ParkingSpotId)
//				.OnDelete(DeleteBehavior.Restrict);

//			base.OnModelCreating(modelBuilder);
//		}

//		public DbSet<User> Users { get; set; }
//		public DbSet<ParkingSpot> ParkingSpots { get; set; }
//		public DbSet<ParkingSetting> parkingSettings { get; set; }
//		public DbSet<ParkingRecord> parkingRecords { get; set; }
//	}
//}
