using System.Data.Entity.ModelConfiguration;
using FriendOrganizer.Model;

namespace FriendOrganizer.DataAccess.EntityConfigurations
{
    public class FriendConfiguration : EntityTypeConfiguration<Friend>
    {
        public FriendConfiguration()
        {
            Property(f => f.FirstName)
                .IsRequired()
                .HasMaxLength(50);


            Property(f => f.LastName)
                .HasMaxLength(50);


            Property(f => f.Email)
                .HasMaxLength(50);
        }
    }
}