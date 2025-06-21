using System.Xml.Serialization;
using CurrencyService.Db.Models;

namespace CurrencyService.Business.Models;

public class Currency
{
  [XmlAttribute("ID")]
  public string Id { get; set; }

  [XmlElement("Name")]
  public string Name { get; set; }

  [XmlElement("VunitRate")]
  public string Rate { get; set; }

  public DbCurrency Map()
  {
    return new DbCurrency
    {
      Id = Id,
      Name = Name,
      Rate = Rate
    };
  }
}
