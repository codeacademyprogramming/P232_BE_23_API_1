using AdminUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace AdminUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private HttpClient _client;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();

        }
        public async Task<IActionResult> Index()
        {
            string token = "Bearer " + Request.Cookies["token"];
            if (token != null) _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

            using (var response = await _client.GetAsync(_configuration["API:BaseUrl"] + "admin/api/products"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<List<ProductItemViewModel>>(content);
                        return View(data);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        return RedirectToAction("login", "account");
                }

            return RedirectToAction("error", "home");
        }

        public async Task<IActionResult> Create()
        {
            var data = await GetBrands();
            ViewBag.Brands = data;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = await GetBrands();
                return View();
            }

            IEnumerable<string> headerToken = null;
            string token = "Bearer " + Request.Cookies["token"];
            if (token != null && !_client.DefaultRequestHeaders.TryGetValues(HeaderNames.Authorization,out headerToken)) _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

            StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            using(var response = await _client.PostAsync(_configuration["API:BaseUrl"] + "admin/api/products", content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var resContent = await response.Content.ReadAsStringAsync();
                    var errors = JsonConvert.DeserializeObject<ErrorReponseViewModel>(resContent);

                    if (errors.Errors != null)
                    {
                        foreach (var item in errors.Errors)
                            ModelState.AddModelError(item.Key, item.Message);
                    }
                    ViewBag.Brands = await GetBrands();
                    return View();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden) 
                    RedirectToAction("login", "account");
                else if(response.StatusCode == System.Net.HttpStatusCode.Created)
                        return RedirectToAction("index");

            }
            return RedirectToAction("error", "home");
        }

        private async Task<List<BrandItemViewModel>> GetBrands()
        {
            IEnumerable<string> headerToken = null;
            string token = "Bearer " + Request.Cookies["token"];
            if (token != null && !_client.DefaultRequestHeaders.TryGetValues(HeaderNames.Authorization, out headerToken)) _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

            List<BrandItemViewModel> data = new List<BrandItemViewModel>();
            using (var response = await _client.GetAsync(_configuration["API:BaseUrl"] + "admin/api/brands"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<BrandItemViewModel>>(content);
                }
            }

            return data;
        }
    }
}
