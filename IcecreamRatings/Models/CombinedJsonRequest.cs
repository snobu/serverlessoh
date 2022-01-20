using System.Collections.Generic;

namespace IcecreamRatings.Models
{

  public class Headers
  {
    public string SalesNumber { get; set; }
    public string DateTime { get; set; }
    public string LocationId { get; set; }
    public string LocationName { get; set; }
    public string LocationAddress { get; set; }
    public string LocationPostcode { get; set; }
    public string TotalCost { get; set; }
    public string TotalTax { get; set; }
  }

  public class Detail
  {
    public string ProductId { get; set; }
    public string Quantity { get; set; }
    public string UnitCost { get; set; }
    public string TotalCost { get; set; }
    public string TotalTax { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
  }


  public class CombinedJsonRequest
  {
    [Newtonsoft.Json.JsonProperty("id")]
    public string Id { get; set; }
    public Headers Headers { get; set; }
    public List<Detail> Details { get; set; }
  }

  public class CombinedJsonModel
  {
    [Newtonsoft.Json.JsonProperty("id")]
    public string Id { get; set; }
    public List<CombinedJsonRequest> CombinedJsonRequest { get; set; }

  }


}