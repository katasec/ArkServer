using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;
using Microsoft.EntityFrameworkCore;

namespace ArkServer.Db
{
    public class RequestsContext:DbContext
    {
        public DbSet<CreateAzureCloudspaceRequest> AzureCloudpsaceRequests { get; set; }
        public string DbPath { get; }

        public RequestsContext()
        {

            // Define DB path & File
            var dbDir = Path.Join(ArkConfig.ArkHome, "db");
            if (!Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }
            var dbFile = Path.Join(dbDir, "ark.db");

            //var path = Ark.Config.
            DbPath = dbFile;
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }
}
