using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class TeamConfiguration : BaseConfiguration<Team, string>
    {
        public override void CustomConfig(EntityTypeBuilder<Team> builder)
        {
            builder.HasMany(p => p.Players)
                .WithOne(t => t.Team)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
