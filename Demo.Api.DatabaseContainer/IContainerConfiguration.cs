namespace Demo.Api.DatabaseContainer
{
    public interface IContainerConfiguration
    {
        public string Datasource { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

    }
}
