namespace Demo.Api.DatabaseContainer
{
    public interface IContainerConfiguration
    {
        public string PortNumber { get; set; }
        public string DatabasePassword { get; set; }

    }
}
