using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS.Core.Interfaces;
using SMS.Infrastructure.Models;

namespace SMS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SMSContext _dbContext;
        public IProductRepository Products { get; }

        public UnitOfWork(SMSContext dbContext,
                            IProductRepository productRepository)
        {
            _dbContext = dbContext;
            Products = productRepository;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

    }
}
