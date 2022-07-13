using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Api.Helper;
using System;
using Api.Models;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class DashboardController : MyBaseController
  {
    private readonly ILogger<DashboardController> _logger;
    private IntegrationExternal _external;

    public DashboardController(ILogger<DashboardController> logger, IntegrationExternal external)
    {
      _logger = logger;
      _external = external;
    }

    [HttpGet]
    public IActionResult Data()
    {
      var response = new List<DashboardResponse>();

      try
      {
        Tuple<DateTime, decimal> result = _external.GetCryptoPrice("BTC");
        if (result != null)
        {
          response.Add(new DashboardResponse()
          {
            ticket = "BTC",
            name = "Bitcoin",
            price = result.Item2.ConvertStringCultureEnUs(),
            syncDate = result.Item1
          });
        }

        result = _external.GetCryptoPrice("ETH");
        if (result != null)
        {
          response.Add(new DashboardResponse()
          {
            ticket = "ETH",
            name = "Ethereum",
            price = result.Item2.ConvertStringCultureEnUs(),
            syncDate = result.Item1
          });
        }

        result = _external.GetCryptoPrice("ADA");
        if (result != null)
        {
          response.Add(new DashboardResponse()
          {
            ticket = "ADA",
            name = "Cardano",
            price = result.Item2.ConvertStringCultureEnUs(),
            syncDate = result.Item1
          });
        }

        result = _external.GetShareNasdaqPrice("GOOGL");
        if (result != null)
        {
          response.Add(new DashboardResponse()
          {
            ticket = "GOOGL",
            name = "Alphabet Inc",
            price = result.Item2.ConvertStringCultureEnUs(),
            syncDate = result.Item1
          });
        }

        result = _external.GetShareNasdaqPrice("MSFT");
        if (result != null)
        {
          response.Add(new DashboardResponse()
          {
            ticket = "MSFT",
            name = "Microsoft Corporation",
            price = result.Item2.ConvertStringCultureEnUs(),
            syncDate = result.Item1
          });
        }

        result = _external.GetShareNasdaqPrice("MARA");
        if (result != null)
        {
          response.Add(new DashboardResponse()
          {
            ticket = "MARA",
            name = "Marathon Digital Holdings Inc",
            price = result.Item2.ConvertStringCultureEnUs(),
            syncDate = result.Item1
          });
        }

        result = _external.GetMetalGoldPrice();
        if (result != null)
        {
          response.Add(new DashboardResponse()
          {
            ticket = "GOLD",
            name = "Gold ounce",
            price = result.Item2.ConvertStringCultureEnUs(),
            syncDate = result.Item1
          });
        }

        return Ok(response);
      }
      catch (Exception ex)
      {
        var msg = "Error get price stock for dashboard";
        _logger.LogError(0, ex, "UserGuid:" + UserGuid + " : " + msg);
        return StatusCode(StatusCodes.Status500InternalServerError, msg);
      }
    }
  }
}
