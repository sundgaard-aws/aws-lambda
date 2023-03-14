using System.Threading.Tasks;

namespace OM.AWS.Demo.SL
{
    public interface IDatabaseService
    {
        public Task SaveAsync<T>(T item);
    }
}