using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.ValueGenerators;

namespace Persistence.Configurations
{
    public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
    {
        public void Configure(EntityTypeBuilder<UserDevice> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(e => e.DeviceToken).IsUnique(true);

            builder.Property(c => c.Id)
            .HasValueGenerator<SeqIdValueGenerator>()
            .ValueGeneratedOnAdd();

            builder
                .HasOne(i => i.User)
                .WithMany(u => u.UserDevices)
                .HasForeignKey(s => s.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
