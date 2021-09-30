using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HobitShorten.Models;
using Microsoft.AspNetCore.Http;
using HobitShorten.Models.Dtos;
using LiteDB;

namespace HobitShorten.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string PostURL(string url)
        {
            try
            {
                //In a work environment this would be an API call instead
                //The API would have the URLShortener as well to make sure any DB interactions are kept away from the frontend.
                if (String.IsNullOrWhiteSpace(url))
                {
                    return "You must enter a URL";
                }

                if (!url.Contains("http"))
                {
                    url = "https://" + url;
                }
                var host = "https://" + _httpContextAccessor.HttpContext.Request.Host.Value;

                URLShortener shortURL = new URLShortener();

                return shortURL.ShortenURL(url, host);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{token}")]
        public IActionResult GetRedirect(string token)
        {
            //Once again this would be an API call, instead of directly interfacing with the DB
            using (var database = new LiteDatabase("Data/Urls.db"))
            {
                return Redirect(database.GetCollection<URLDto>().FindOne(f => f.Token == token).URL);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
