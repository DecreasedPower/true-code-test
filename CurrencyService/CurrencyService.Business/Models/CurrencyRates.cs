using System.Xml.Serialization;

namespace CurrencyService.Business.Models;

[XmlRoot("ValCurs")]
public class CurrencyRates
{
  [XmlElement("Valute")]
  public List<Currency> Currencies { get; set; }
}
