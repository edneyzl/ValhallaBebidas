using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

[Route("api/cliente")]
[ApiController]
public class ClienteApiController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ClienteApiController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

}