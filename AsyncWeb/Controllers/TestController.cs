using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AsyncWeb
{
    public class TestController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/proxy/{uri?}")]
        public IActionResult Data(string uri)
        {
            var dataSource = uri == null ? "https://arghya.xyz" : $"http://{uri}";
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(dataSource).Result;
                return Content(response.Content.ReadAsStringAsync().Result, "text/html");
            }
        }

        [HttpGet("/smartproxy/{uri?}")]
        public async Task<IActionResult> Data2(string uri)
        {
            HttpContext.Session.SetString("ac", "bwahahahaha");
            var sessionAc = HttpContext.Session.GetString("ac");
            var dataSource = uri == null ? "https://arghya.xyz" : $"http://{uri}";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(dataSource).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var sessionAc2 = HttpContext.Session.GetString("ac"); //still works even with ConfigureAwait(false)
                return Content(content, "text/html");
            }
        }

        [HttpGet("/smartproxy2")]
        public async Task<IActionResult> GetExternalData()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://arghya.xyz");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "text/html");
            }
        }
    }
}
