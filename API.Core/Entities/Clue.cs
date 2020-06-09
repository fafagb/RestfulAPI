using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.Entities
{
    public class Clue : Entity
    {
       /// <summary>
       /// 事故描述
       /// </summary>
        public string Dangerarea { get; set; }

        public string Mobile { get; set; }

        public string ReportCasePeople { get; set; }

        public DateTime LastModified { get; set; }


    }
}
