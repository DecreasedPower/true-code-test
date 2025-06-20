using System.Xml.Serialization;

namespace CurrencyService.Business.Models;

[XmlRoot("ValCurs")]
public class ValuteCurrency
{
  [XmlElement("Valute")]
  public List<Valute> Valutes { get; set; }
}
