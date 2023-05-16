using NadoMapper.Interfaces;

namespace NadoMapper.Models
{
  public record Test : ModelBase, IModel
  {
    public string Name { get; set; }
  }
}