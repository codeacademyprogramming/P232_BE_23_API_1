using AdminUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AdminUI.Controllers
{
    public class BrandController : Controller
    {
        private HttpClient _client;
        private readonly IConfiguration _configuration;

        public BrandController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();

           
        }
        public async Task<IActionResult> Index()
        {
            string token = "Bearer "+ Request.Cookies["token"];
            if (token != null) _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
            List<BrandItemViewModel> data = new List<BrandItemViewModel>();
            using (var response = await _client.GetAsync("https://localhost:7137/admin/api/Brands"))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseStr = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<BrandItemViewModel>>(responseStr);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    return RedirectToAction("login", "account");
                else
                    return RedirectToAction("error", "home");
            }
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateViewModel brand)
        {
            if (!ModelState.IsValid) return View();

            string token = "Bearer " + Request.Cookies["token"];
            if (token != null) _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
            StringContent content = new StringContent(JsonConvert.SerializeObject(brand), Encoding.UTF8, "application/json");
            using (var response = await _client.PostAsync("https://localhost:7137/admin/api/Brands", content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var resContent = await response.Content.ReadAsStringAsync();
                    ErrorReponseViewModel err = JsonConvert.DeserializeObject<ErrorReponseViewModel>(resContent);

                    if (err.Errors != null)
                    {
                        foreach (var item in err.Errors)
                            ModelState.AddModelError(item.Key, item.Message);
                    }

                    return View();
                }
            }

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            string token = "Bearer " + Request.Cookies["token"];
            if (token != null) _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
            BrandEditViewModel data = new BrandEditViewModel();
            using (var response = await _client.GetAsync("https://localhost:7137/admin/api/Brands/" + id))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return RedirectToAction("error", "home");

                string responseStr = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<BrandEditViewModel>(responseStr);
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BrandEditViewModel brand)
        {
            string token = "Bearer " + Request.Cookies["token"];
            if (token != null) _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
            StringContent content = new StringContent(JsonConvert.SerializeObject(brand), Encoding.UTF8, "application/json");
            using (var response = await _client.PutAsync("https://localhost:7137/admin/api/Brands/" + id, content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var resContent = await response.Content.ReadAsStringAsync();
                    ErrorReponseViewModel err = JsonConvert.DeserializeObject<ErrorReponseViewModel>(resContent);

                    if (err.Errors != null)
                    {
                        foreach (var item in err.Errors)
                            ModelState.AddModelError(item.Key, item.Message);
                    }
                    return View();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return RedirectToAction("error", "home");
                }
            }

            return RedirectToAction("index");
        }
    }
}
