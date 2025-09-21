using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Controllers;

public class RESTControllerBase : ControllerBase
{
    protected int GetPageSize(int pageSize, int minPageSize, int maxPageSize) => Math.Clamp(pageSize, minPageSize, maxPageSize);
}
