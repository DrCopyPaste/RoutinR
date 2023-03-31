using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoutinR.SQLite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutinR.SQLite
{
    public class RoutinRContext : DbContext
    {
        // constructor for ef migration tooling
        public RoutinRContext() : base(new DbContextOptionsBuilder().UseSqlite("Data Source=:memory:").Options)
        {

        }

        public RoutinRContext(SqliteConnection sqliteConnection) : base(new DbContextOptionsBuilder().UseSqlite(sqliteConnection).Options)
        {

        }

        public RoutinRContext(string connectionString) : base(new DbContextOptionsBuilder().UseSqlite(connectionString).Options)
        {

        }

        //public DbSet<ApiExportProfile> ApiExportProfiles { get; set; }
        //public DbSet<ExportResult> ExportResults { get; set; }
        public DbSet<Job> Jobs { get; set; }   
        public DbSet<TimeSheetEntry> TimeSheetEntries { get; set; }
    }
}
