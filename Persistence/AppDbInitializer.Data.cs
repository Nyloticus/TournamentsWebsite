using System.Threading.Tasks;

namespace Persistence
{
    public partial class AppDbInitializer
    {

        public async Task SeedEverything()
        {

            await SeedAllData();

        }
        private async Task SeedAllData()
        {


            await _context.SaveChangesAsync();
        }
    }
}