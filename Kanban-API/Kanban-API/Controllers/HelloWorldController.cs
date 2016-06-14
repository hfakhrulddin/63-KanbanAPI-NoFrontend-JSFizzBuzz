using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kanban_API.Controllers
{
    public class HelloWorldController : ApiController
    {
        public string GET()
        {
            return "Hello World";
        }
    }
}
