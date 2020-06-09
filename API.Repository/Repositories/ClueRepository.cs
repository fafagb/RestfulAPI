using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Core.Entities;
using API.Core.Interfaces;
using API.Repository.Database;
using API.Repository.Extensions;
using API.Repository.Resources;
using API.Repository.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.Repositories
{
    public class ClueRepository : IClueRepository
    {
        private readonly MANBU_qaContext _qaContext;
   

        private readonly IPropertyMappingContainer _propertyMappingContainer;
        public ClueRepository(MANBU_qaContext qaContext, IPropertyMappingContainer propertyMappingContainer)
        {
            _qaContext = qaContext;
            _propertyMappingContainer = propertyMappingContainer;
        }

        public void AddClue(TxClues clue)
        {
            _qaContext.Add(clue);
        }

        public void Delete(TxClues clue)
        {
            _qaContext.Remove(clue);
        }

        public async Task<PaginatedList<TxClues>> GetAllCluesAsync(ClueParameters clueParameters)
        {
            
            var query = _qaContext.Clues.AsQueryable();

            if (!string.IsNullOrEmpty(clueParameters.Mobile))
            {
                string mobile = clueParameters.Mobile.ToLowerInvariant();
                query=query.Where(x => x.Mobile.ToLowerInvariant()== mobile);
            }
            //query = query.OrderBy(x => x.Id);
            //ApplySort:API.Repository.Extensions;
            query = query.ApplySort(clueParameters.OrderBy, _propertyMappingContainer.Resolve<ClueResource, TxClues>());


            var count = await query.CountAsync();

            var data = await query.Skip(clueParameters.PageIndex * clueParameters.PageSize).Take(clueParameters.PageSize).ToListAsync();

            return new PaginatedList<TxClues>(clueParameters.PageIndex, clueParameters.PageSize, count, data);
 
        }



        public async Task<TxClues> GetClueByIdAsync(int id)
        {
            return await _qaContext.Clues.FindAsync(id);
        }

        public void Update(TxClues clue)
        {
            _qaContext.Entry(clue).State=EntityState.Modified;
        }


    }
}
