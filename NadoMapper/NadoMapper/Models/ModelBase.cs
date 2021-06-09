using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NadoMapper.Models
{
    public class ModelBase
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }
        public byte[] LastModified { get; set; }
    }
}