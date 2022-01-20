using Newtonsoft.Json;

namespace Company.Function
{
  public class RequestModel
  {
    [JsonProperty("orderHeaderDetailsCSVUrl")]
    public string OrderHeaderDetailsCSVUrl { get; set; }
    [JsonProperty("orderLineItemsCSVUrl")]
    public string OrderLineItemsCSVUrl { get; set; }
    [JsonProperty("productInformationCSVUrl")]
    public string ProductInformationCSVUrl { get; set; }
  }

}