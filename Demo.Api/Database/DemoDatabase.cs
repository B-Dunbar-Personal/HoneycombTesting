using Dapper;
using Microsoft.Data.SqlClient;

namespace Demo.Api.Database
{
    public class DemoDatabase : IDemoDatabase
    {
        private AppSettings _appsettings;

        public DemoDatabase(IConfiguration configuration)
        {
            _appsettings = configuration.GetSection("AppSettings").Get<AppSettings>();

        }

        private SqlConnection CreateConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = _appsettings.Datasource;
            builder.UserID = _appsettings.UserId;
            builder.Password = _appsettings.Password;
            builder.TrustServerCertificate = true;
            builder.InitialCatalog = "master";
            return new SqlConnection(builder.ConnectionString);
        }

        public async Task<IEnumerable<Person>> GetIndividualsAddressLines(int personId)
        {
            var connection = CreateConnection();
            using (connection)
            {
                string sql = "SELECT Address1, Address2, City FROM Person WHERE PersonId = @personId";
                return await connection.QueryAsync<Person>(sql, new { personId });
            }

        }
    }
}
