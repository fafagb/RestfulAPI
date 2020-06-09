using System;
using System.Collections.Generic;
using System.Text;

namespace API.Repository.Resources
{
   public  class PersonAddOrUpdateResource
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public int? Sex { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
