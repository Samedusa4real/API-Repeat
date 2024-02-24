using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.UI.Models;
using System.Net.Http;

namespace Shop.UI.Controllers
{
    public class AccountController : Controller
    {
        readonly HttpClient _client;
        public AccountController()
        {
            _client = new HttpClient();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest login, string? returnUrl)
        {
            if (!ModelState.IsValid)
                return View();

            StringContent content = new StringContent(JsonConvert.SerializeObject(login), System.Text.Encoding.UTF8, "application/json");
            using(var response = await _client.PostAsync("https://localhost:7000/api/auth/login", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var token = "Bearer " + responseContent;

                    HttpContext.Response.Cookies.Append("auth_token", token);

                    return returnUrl == null ? RedirectToAction("Index", "Home") : Redirect(returnUrl);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError("", "Email or password is not correct!");
                    return View();
                }

            }

            return View("Error");
        }
    }
}
