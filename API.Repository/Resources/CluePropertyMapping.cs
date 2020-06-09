using System;
using System.Collections.Generic;
using System.Text;
using API.Core.Entities;
using API.Repository.Services;

namespace API.Repository.Resources
{
    public class CluePropertyMapping : PropertyMapping<ClueResource, TxClues>
    {
        public CluePropertyMapping() :
            base(

                new Dictionary<string, List<MappedProperty>>(StringComparer.OrdinalIgnoreCase)

                {
                    [nameof(ClueResource.Dangerarea)] = new List<MappedProperty>
                    {
                          new MappedProperty{  Name=nameof(TxClues.Dangerarea), Revert=false}//排序无相反
                    },
                    [nameof(ClueResource.Mobile)] = new List<MappedProperty>
                    {
                          new MappedProperty{  Name=nameof(TxClues.Mobile), Revert=false}
                    },
                    [nameof(ClueResource.ReportCasePeople)] = new List<MappedProperty>
                    {
                          new MappedProperty{  Name=nameof(TxClues.ReportCasePeople), Revert=false}
                    }

                })







        {

        }



    }
}
