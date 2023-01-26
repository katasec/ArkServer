using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ArkServer.Entities.Azure;
using ServiceStack.Data;

OrmLiteConnectionFactory getDbFactory()
{
    var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    var dbFile = Path.Join(homeDir, ".ark", "db", "ark2.db");
    return new OrmLiteConnectionFactory(dbFile, SqliteDialect.Provider);
}
void InsertStuff()
{
    using (var db = getDbFactory().Open())
    {
        db.CreateTableIfNotExists<AzureCloudspace>();

        for (int i = 0; i <= 10; i++)
        {
            var acs = new AzureCloudspace()
                        .AddSpoke($"ameer{i}")
                        .AddSpoke($"egal{i}");
            db.Insert(acs);
        }
    }
}

void PrintStuff()
{
    IDbConnectionFactory db = getDbFactory();
    using var db2 = db.Open();
    var acs2 = db2.LoadSingleById<AzureCloudspace>("b2ee0389-0809-4521-9c29-296bc9a04504");
    acs2.Spokes.ToList().ForEach(x => Console.WriteLine(x.Name));
}


PrintStuff();


