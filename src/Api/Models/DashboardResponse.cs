using System;

namespace Api.Models
{
  public class DashboardResponse
  {
    public string ticket { get; set; }
    public string name { get; set; }
    public string price { get; set; }
    public DateTime syncDate { get; set; }
  }
}
