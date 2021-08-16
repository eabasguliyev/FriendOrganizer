using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess.EntityConfigurations;
using FriendOrganizer.Model;

namespace FriendOrganizer.DataAccess
{
    public class FriendOrganizerDbContext:DbContext
    {
        public DbSet<Friend> Friends { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }

        public DbSet<FriendPhoneNumber> FriendPhoneNumbers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }


        public FriendOrganizerDbContext():base("FriendOrganizerDb")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new FriendConfiguration());

            modelBuilder.Entity<ProgrammingLanguage>().Property(pl => pl.Name).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<FriendPhoneNumber>().Property(pn => pn.Number).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Meeting>().Property(m => m.Title).IsRequired().HasMaxLength(50);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync()
        {
            var objContext = ((IObjectContextAdapter)this).ObjectContext;
            var entries = objContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified)
                .Select(entry => entry.Entity);
            var str = typeof(string).Name;


            foreach (var entity in entries)
            {
                var properties = from p in entity.GetType().GetProperties()
                    where p.PropertyType.Name == str
                    select p;
                var items = from item in properties
                    let value = (string)item.GetValue(entity, null)
                    where value != null && value.Trim().Length == 0
                    select item;
                foreach (var item in items)
                {
                    item.SetValue(entity, null, null);
                }
            }
            return base.SaveChangesAsync();
        }
    }
}