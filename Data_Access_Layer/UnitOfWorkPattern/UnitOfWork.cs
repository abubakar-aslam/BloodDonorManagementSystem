using Data_Access_Layer.DataContext;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.Repository;

namespace Data_Access_Layer.UnitOfWorkPattern
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext dbContext;
        private IDonorRepository donorRepository;
        public UnitOfWork(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }
        public IDonorRepository DonorRepository
        {
            get
            {
                if (donorRepository == null)
                    donorRepository = new DonorRepository(dbContext);
                return donorRepository;
            }
        }
        public void Save()
        {
            dbContext.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }

                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
