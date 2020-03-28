using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Client.Controllers
{
    public class AuthorizationOptionsController:Controller
    {

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
