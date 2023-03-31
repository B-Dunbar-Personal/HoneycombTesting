using Dapper;
using Microsoft.Data.SqlClient;

namespace Demo.Api.DatabaseContainer
{
    public class SqlContainer : DockerContainer
    {
        public SqlContainer(IContainerConfiguration containerConfiguration) : base(containerConfiguration)
        {

        }

        public SqlConnection CreateConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ContainerConfiguration.Datasource;
            builder.UserID = ContainerConfiguration.UserId;
            builder.Password = ContainerConfiguration.Password;
            builder.TrustServerCertificate = true;

            return new SqlConnection(builder.ConnectionString);
        }

        public override async Task ExecuteNonQuery(string sql)
        {
            var connection = CreateConnection();
            using (connection)
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql);
                await connection.CloseAsync();
            }
        }

        public override async Task ExecuteNonQuery(string sql, object model)
        {
            var connection = CreateConnection();
            using (connection)
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, model);
                await connection.CloseAsync();
            }
        }
    }
}