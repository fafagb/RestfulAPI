using System;
using System.Collections.Generic;

namespace API.Core.Entities
{
    public partial class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public int? Sex { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
