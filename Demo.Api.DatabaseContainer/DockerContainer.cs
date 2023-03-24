namespace Demo.Api.DatabaseContainer
{
    public abstract class DockerContainer
    {
        internal string ContainerId;
        internal IContainerConfiguration ContainerConfiguration;

        public DockerContainer(IContainerConfiguration containerConfiguration)
        {

            ContainerId = string.Empty;
            ContainerConfiguration = containerConfiguration;
        }

        public abstract Task ExecuteNonQuery(string query, object model);

        public abstract Task ExecuteNonQuery(string query);

    }
}