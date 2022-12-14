using System.ComponentModel;
using System.Reflection;
using Template.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Template.Infrastructure.DataAccess
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> GetNextSequence(Sequence sequence)
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var sequenceIdentifier = sequence.GetType()
                        .GetMember(sequence.ToString())
                        .First()
                        .GetCustomAttribute<DescriptionAttribute>()
                        ?.Description;
            await Database.ExecuteSqlRawAsync($"SELECT @result = (NEXT VALUE FOR [{sequenceIdentifier}])", result);
            return (int)result.Value;
        }
    }
}