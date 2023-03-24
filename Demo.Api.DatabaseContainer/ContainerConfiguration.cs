namespace Demo.Api.DatabaseContainer
{
    public class ContainerConfiguration : IContainerConfiguration
    {
        public string PortNumber { get; set; }
        public string DatabasePassword { get; set; }

    }
}