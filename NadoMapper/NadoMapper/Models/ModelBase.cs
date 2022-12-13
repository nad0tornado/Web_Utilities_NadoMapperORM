using System;

namespace NadoMapper.Models
{
  public record ModelBase
  {
    public int Id { get; set; }

    public DateTime DateAdded { get; set; }
    public byte[] LastModified { get; set; }
  }
}