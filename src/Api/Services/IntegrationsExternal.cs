using System.Collections.Generic;
using System.Threading;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class IntegrationExternal
{
  private readonly List<KeyValuePair<string, string>> cryptoAssetAvaible = new List<KeyValuePair<string, string>>() {
    new KeyValuePair<string, string>("BTC", "bitcoin"),
    new KeyValuePair<string, string>("ETH", "ethereum"),
    new KeyValuePair<string, string>("ADA", "cardano")
    // https://api.coingecko.com/api/v3/simple/price?ids=XXXXXX&vs_currencies=usd
  };
  private readonly string[] nasdaqShareAssetAvaible = { "GOOGL", "MSFT", "MARA" };
  private IMemoryCache _cache;

  public IntegrationExternal(IMemoryCache cache)
  {
    _cache = cache;
  }

  public Tuple<DateTime, decimal> GetCryptoPrice(string asset)
  {
    asset = asset.ToLower();
    Tuple<DateTime, decimal> result = null;

    var isAvaible = cryptoAssetAvaible.Exists(x => x.Key.ToLower() == asset);

    if (isAvaible && !_cache.TryGetValue(asset, out result))
    {
      try
      {
        Thread.Sleep(250);

        var id = cryptoAssetAvaible.Find(x => x.Key.ToLower() == asset).Value;
        var respuesta = CoinGeckoCryptoMarketData(id).Result;
        var json = (JObject)JsonConvert.DeserializeObject(respuesta);

        var jsonStr = json[id]["usd"].ConvertStringCultureEnUs();
        if (!string.IsNullOrEmpty(jsonStr))
        {
          result = new Tuple<DateTime, decimal>(DateTime.Now, jsonStr.ConvertDecimalCultureEnUs());

          var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

          _cache.Set(asset, result, cacheEntryOptions);
        }
      }
      catch (System.Exception)
      {
        //loguear error
      }
    }

    return result;
  }

  public Tuple<DateTime, decimal> GetShareNasdaqPrice(string asset)
  {
    asset = asset.ToLower();
    Tuple<DateTime, decimal> result = null;

    var isAvaible = Array.Exists(nasdaqShareAssetAvaible, element => element.ToLower() == asset);

    if (isAvaible && !_cache.TryGetValue(asset, out result))
    {
      try
      {
        Thread.Sleep(500);

        var respuesta = NasdaqMarketData(asset).Result;
        var json = (JObject)JsonConvert.DeserializeObject(respuesta);

        var jsonStr = json["data"]["primaryData"]["lastSalePrice"].ConvertStringCultureEnUs();
        if (!string.IsNullOrEmpty(jsonStr))
        {
          result = new Tuple<DateTime, decimal>(DateTime.Now, jsonStr.Replace('$', ' ').ConvertDecimalCultureEnUs());

          var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

          _cache.Set(asset, result, cacheEntryOptions);
        }
      }
      catch (System.Exception)
      {
        //loguear error
      }
    }

    return result;
  }

  public Tuple<DateTime, decimal> GetMetalGoldPrice()
  {
    const string gold = "gold";
    Tuple<DateTime, decimal> result = null;


    if (!_cache.TryGetValue(gold, out result))
    {
      try
      {
        var respuesta = MetalMarketData().Result;
        var json = (JObject)JsonConvert.DeserializeObject(respuesta);
        result = new Tuple<DateTime, decimal>(DateTime.Now, json[gold].ConvertDecimalCultureEnUs());

        var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

        _cache.Set(gold, result, cacheEntryOptions);
      }
      catch (System.Exception)
      {
        //loguear error
      }
    }

    return result;
  }

  // Documentation: https://www.coingecko.com/es/api/documentation
  // https://api.coingecko.com/api/v3/simple/price?ids=axie-infinity&vs_currencies=usd
  private async Task<string> CoinGeckoCryptoMarketData(string id)
  {
    string url = $"https://api.coingecko.com/api/v3/simple/price?ids={id}&vs_currencies=usd";
    using (var client = new HttpClient())
      return await client.GetStringAsync(url);
  }

  private async Task<string> NasdaqMarketData(string asset)
  {
    string url = $"https://api.nasdaq.com/api/quote/{asset}/info?assetclass=stocks";
    using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip }))
    {
      client.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
      return await client.GetStringAsync(url);
    }
  }

  private async Task<string> MetalMarketData()
  {
    string url = $"https://services.packetizer.com/spotprices/?f=json";
    using (var client = new HttpClient())
      return await client.GetStringAsync(url);
  }
}