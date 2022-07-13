using System;
using System.Globalization;

namespace Api.Helper
{
  public static class StringExtensions
  {
    public static decimal ConvertDecimalCultureEnUs(this string str)
    {
      return decimal.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
  }
}