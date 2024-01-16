using Data_Access_Layer.Interfaces;

namespace Data_Access_Layer.Interfaces
{
    public interface IUnitOfWork
    {
        IDonorRepository DonorRepository { get; }
        void Save();
        void Dispose();
    }
}
