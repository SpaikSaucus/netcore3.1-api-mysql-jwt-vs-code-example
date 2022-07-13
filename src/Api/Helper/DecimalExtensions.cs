using System;
using System.Globalization;

namespace Api.Helper
{
  public static class DecimalExtensions
  {
    public static string ConvertStringCultureEnUs(this decimal? dec)
    {
      if (dec.HasValue)
        return ConvertStringCultureEnUs(dec.Value);
      else
        return "0";
    }
    public static string ConvertStringCultureEnUs(this decimal dec)
    {
      return Convert.ToString(dec, CultureInfo.InvariantCulture);
    }
  }
}