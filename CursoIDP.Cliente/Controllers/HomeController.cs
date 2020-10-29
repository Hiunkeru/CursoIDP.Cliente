using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CursoIDP.Cliente.Models;
using Microsoft.AspNetCore.Authorization;

namespace CursoIDP.Cliente.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Usuario()
        {
            var httpClient = _clientFactory.CreateClient("APIClient");
            var response = await httpClient.GetAsync("api/usuario").ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var companiesString = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<UsuarioModel>>(companiesString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(users);
        }
    }
}
