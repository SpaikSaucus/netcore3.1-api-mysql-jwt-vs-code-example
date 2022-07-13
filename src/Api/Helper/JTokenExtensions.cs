using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Api.Helper
{
  public static class JTokenExtensions
  {
    public static decimal ConvertDecimalCultureEnUs(this JToken json)
    {
      return Convert.ToDecimal(json, new CultureInfo("en-US"));
    }
    public static string ConvertStringCultureEnUs(this JToken json)
    {
      return Convert.ToString(json, new CultureInfo("en-US"));
    }
  }
}