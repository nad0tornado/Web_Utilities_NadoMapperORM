using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NadoMapper.Models
{
  public record Test : ModelBase
  {
    public string Name { get; set; }
  }
}