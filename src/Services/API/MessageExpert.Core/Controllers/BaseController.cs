using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Core.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BaseController : Controller
    {
    }
}
