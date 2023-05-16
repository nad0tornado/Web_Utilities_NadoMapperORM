using NadoMapper.Interfaces;
using System;

namespace NadoMapper.Models
{
  public record ModelBase : IModel
  {
    public int Id { get; set; }

    public DateTime DateAdded { get; set; }

    public byte[] LastModified { get; set; }
  }
}