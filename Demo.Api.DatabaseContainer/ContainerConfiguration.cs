namespace Demo.Api.DatabaseContainer
{
    public class ContainerConfiguration : IContainerConfiguration
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Datasource {get; set; }
    }
}