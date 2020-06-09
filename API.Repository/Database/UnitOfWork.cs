using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using API.Core.Entities;
using API.Core.Interfaces;

namespace API.Repository.Database
{
    public class UnitOfWork : IUnitOfWork
    {
       private readonly mydbContext _qaContext;
        public UnitOfWork(mydbContext qaContext)
        {
            _qaContext = qaContext;
        }

        public async Task<bool> SaveAsync()
        {
            return await _qaContext.SaveChangesAsync()>0;
        }
    }
}
