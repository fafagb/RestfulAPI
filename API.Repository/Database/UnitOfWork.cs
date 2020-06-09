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
       private readonly MANBU_qaContext _qaContext;
        public UnitOfWork(MANBU_qaContext qaContext)
        {
            _qaContext = qaContext;
        }

        public async Task<bool> SaveAsync()
        {
            return await _qaContext.SaveChangesAsync()>0;
        }
    }
}
