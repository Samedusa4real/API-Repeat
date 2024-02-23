using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.UI.Models;

namespace Shop.UI.Controllers
{
    public class BrandController : Controller
    {
        private HttpClient _client;
        public BrandController()
        {
            _client = new HttpClient();
        }
        public async Task<IActionResult> Index()
        {
            using (var response = await _client.GetAsync("https://localhost:7000/api/Brands/all"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<BrandGetAllItemResponse>>(content);

                    return View(data);
                }
            }
            return View("error");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateRequest brand)
        {
            if (!ModelState.IsValid) return View();

            StringContent content = new StringContent(JsonConvert.SerializeObject(brand), System.Text.Encoding.UTF8, "application/json");
            using (var response = await _client.PostAsync("https://localhost:7000/api/Brands", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorResponseModel>(responseContent);

                    foreach (var item in error.Errors)
                        ModelState.AddModelError(item.Key, item.Message);

                    return View();
                }
            }

            return View("error");
        }

        public async Task<IActionResult> Edit(int id)
        {
            using (var response = await _client.GetAsync($"https://localhost:7000/api/Brands/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<BrandGetResponse>(responseContent);
                    var vm = new BrandUpdateRequest { Name = data.Name };

                    return View(vm);
                }
            }

            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandUpdateRequest brand)
        {
            if (!ModelState.IsValid) 
                return View();

            StringContent content = new StringContent(JsonConvert.SerializeObject(brand), System.Text.Encoding.UTF8, "application/json");
            using (var response = await _client.PutAsync($"https://localhost:7000/api/Brands/{id}", content))
            {
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorResponseModel>(responseContent);

                    foreach (var item in error.Errors)
                        ModelState.AddModelError(item.Key, item.Message);

                    return View();

                }
            }

            return View("Error");
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var response = await _client.DeleteAsync($"https://localhost:7000/api/Brands/{id}"))
            {
                if (response.IsSuccessStatusCode)
                    return Ok();
            }

            return NotFound();
        }
    }
}
