namespace DockerIntegrationTesting.Tests.Scripts
{
    public static class SqlFileReader
    {
        public static async Task<string> GetSqlFile(string file)
        {
            var directory = @$"{Directory.GetCurrentDirectory()}/Scripts/{file}.sql";
            string text = await File.ReadAllTextAsync(directory);
            return text;
        }
    }
}
