using System.Xml.Serialization;

namespace CurrencyService.Business.Models;

public class Valute
{
  [XmlAttribute("ID")]
  public string Id { get; set; }

  [XmlElement("Name")]
  public string Name { get; set; }

  [XmlElement("VunitRate")]
  public string Rate { get; set; }
}
