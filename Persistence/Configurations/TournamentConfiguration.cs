using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class TournamentConfiguration : BaseConfiguration<Tournament, string>
    {
        public override void CustomConfig(EntityTypeBuilder<Tournament> builder)
        {

        }
    }
}
