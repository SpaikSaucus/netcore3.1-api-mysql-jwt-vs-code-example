using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
  public class MyBaseController : ControllerBase
  {
    protected string UserGuid { get => HttpContext.Items["userGuid"].ToString(); }
  }
}