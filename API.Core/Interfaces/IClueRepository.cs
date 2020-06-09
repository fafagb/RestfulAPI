using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using API.Core.Entities;

namespace API.Core.Interfaces
{
    public interface IClueRepository
    {
         Task<PaginatedList<TxClues>> GetAllCluesAsync(ClueParameters clueParameters);

        void AddClue(TxClues clue);


        Task<TxClues> GetClueByIdAsync(int id);

        void Delete(TxClues clue);


        void Update(TxClues clue);



    }
}
