using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.UI.Models;

namespace Shop.UI.Controllers
{
    public class BrandController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using(HttpClient client = new HttpClient())
            {
                using(var response = await client.GetAsync("https://localhost:7000/api/Brands/all"))
                {
                    if(response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<List<BrandGetAllResponse>>(content);

                        return View(data);
                    }
                }
            }
            return View("error");
        }
    }
}
