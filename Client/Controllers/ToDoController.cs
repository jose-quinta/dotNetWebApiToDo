using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers {
    public class ToDoController : Controller {

        private readonly HttpClient _http = new HttpClient();
        private readonly HttpClientHandler clientHandler = new HttpClientHandler();
        private readonly string url = "https://localhost:7299/api/ToDo/";

        public ToDoController() {
            clientHandler.ServerCertificateCustomValidationCallback =
                (sender, cert, chain, sslPolicyErrors) => { return true; };
        }

        public async Task<ActionResult<List<ToDo>>> Index() {
            List<ToDo> response = new List<ToDo>();
            using (var _api = await _http.GetAsync(url)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<ToDo>>(_response)!;
            }
            return View(response);
        }

        public async Task<ActionResult<ToDo>> Details(int id) {
            ToDo response = new ToDo();
            using (var _api = await _http.GetAsync(url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ToDo>(_response)!;
            }
            return PartialView("_ToDoDetails", response);
        }

        public ActionResult Create() {
            return PartialView("_ToDoCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoDto request) {
            List<ToDo> response = new List<ToDo>();
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            using (var _api = await _http.PostAsync(url, content)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<ToDo>>(_response)!;
            }
            if (response != null)
                return RedirectToAction(nameof(Index));
            return View();
        }

        public async Task<ActionResult<ToDo>> Edit(int id) {
            ToDo response = new ToDo();
            using (var _api = await _http.GetAsync(url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ToDo>(_response)!;
            }
            return PartialView("_ToDoEdit", response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ToDoDto request) {
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var _api = await _http.PutAsync(url + id, content);
            if (_api.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Toogle(int id, string title, string description, string done){
            ToDoDto request = new ToDoDto() {
                Title = title,
                Description = description,
                Done = done == "true" ? true : false
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var _api = await _http.PutAsync(url + id, content);
            if (_api.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
            return View();
        }

        public async Task<ActionResult<ToDo>> Delete(int id) {
            ToDo response = new ToDo();
            using (var _api = await _http.GetAsync(url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ToDo>(_response)!;
            }
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ToDo request) {
            try {
                using (var _api = await _http.DeleteAsync(url + request.Id)) {
                    string response = await _api.Content.ReadAsStringAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch {
                return View();
            }
        }
    }
}