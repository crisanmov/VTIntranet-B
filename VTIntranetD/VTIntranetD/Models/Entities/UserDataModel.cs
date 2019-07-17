namespace VTIntranetD.Models.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UserDataModel : DbContext
    {
        public UserDataModel()
            : base("name=DB_Entities")
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<ProfileTagDepto> ProfileTagDepto { get; set; }
        public virtual DbSet<Event> Event { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
