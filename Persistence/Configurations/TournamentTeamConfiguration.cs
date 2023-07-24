using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class TournamentTeamConfiguration : BaseConfiguration<TournamentTeam, string>
    {
        public override void CustomConfig(EntityTypeBuilder<TournamentTeam> builder)
        {
            builder.Ignore(i => i.Id);

            builder
                .HasKey(tt => new { tt.TournamentId, tt.TeamId });

            builder
                .HasOne(tt => tt.Tournament)
                .WithMany(t => t.TournamentTeams)
                .HasForeignKey(tt => tt.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(tt => tt.Team)
                .WithMany(t => t.TournamentTeams)
                .HasForeignKey(tt => tt.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
