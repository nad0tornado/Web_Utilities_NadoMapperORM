using System;
using System.Collections.Generic;
using System.Text;

namespace NadoMapper.Interfaces
{
    public interface IModel
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }

        public byte[] LastModified { get; set; }
    }
}
