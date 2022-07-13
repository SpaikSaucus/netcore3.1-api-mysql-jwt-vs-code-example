using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Api.Helper
{
  public static class HttpExtensions
  {
    public static string GetHeader(this HttpRequest request, string key)
    {
      return request.Headers.FirstOrDefault(x => x.Key == key).Value.FirstOrDefault();
    }
  }
}