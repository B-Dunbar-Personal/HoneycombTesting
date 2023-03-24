namespace Demo.Api.Database
{
    public interface IDemoDatabase
    {
        Task<IEnumerable<Person>> GetIndividualsAddressLines(int id);
    }
}
