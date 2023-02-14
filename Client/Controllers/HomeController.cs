using Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Client.Controllers {
    public class HomeController : Controller {

        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _http = new HttpClient();
        private readonly HttpClientHandler clientHandler = new HttpClientHandler();
        private readonly string url = "https://localhost:7299/api/ToDo/";

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
            clientHandler.ServerCertificateCustomValidationCallback =
                (sender, cert, chain, sslPolicyErrors) => { return true; };
        }

        public async Task<ActionResult<List<ToDo>>> Index() {
            List<ToDo> response = new List<ToDo>();
            using (var _api = await _http.GetAsync(url)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<ToDo>>(_response)!;
            }
            // var data = from item in collection where item.FieldName.Contains("Text") select item;
            return View(response.Where(x => x.Done == false));
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}