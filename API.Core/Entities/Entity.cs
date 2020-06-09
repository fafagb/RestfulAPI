using System;
using System.Collections.Generic;
using System.Text;
using API.Core.Interfaces;
namespace API.Core.Entities
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
