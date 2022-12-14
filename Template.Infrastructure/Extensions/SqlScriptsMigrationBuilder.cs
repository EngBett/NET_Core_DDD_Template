using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

namespace Template.Infrastructure.Extensions
{
    public static class SqlScriptsMigrationBuilder
    {
        public static void MigrateSqlScripts(this MigrationBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().
                        Where(file => file.EndsWith(".sql"));
            foreach (var sqlFile in sqlFiles)
            {
                using (Stream stream = assembly.GetManifestResourceStream(sqlFile))
                using (StreamReader reader = new StreamReader(stream))
                {
                    var sqlScript = reader.ReadToEnd();
                    builder.Sql(sqlScript);
                }
            }
        }
    }
}
