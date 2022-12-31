using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExampleAPI.Models;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ExampleAPI.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _config;
    private readonly ILogger<HomeController> _logger;

    string baseUrl = "https://api.nal.usda.gov/fdc/v1/";
    string dataType = "Foundation";
    string pageSize = "20";

    public HomeController(ILogger<HomeController> logger, IConfiguration config)
    {
        _config = config;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SearchByQuery(string query)
    {
        // Calling the web API and populating the data in view using Entity Model class
        SearchBase foods = new SearchBase();
       

        using (var client = new HttpClient())
        {
            var apiKey = _config["USDA:ApiKey"];
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage getData = await client.GetAsync($"foods/search?query={query}&api_key={apiKey}&pageSize={pageSize}&dataType={dataType}");

            if (getData.IsSuccessStatusCode)
            {
                string results = getData.Content.ReadAsStringAsync().Result;
                Console.WriteLine(results);
                //foods = JsonConvert.DeserializeObject<Food>(results);
                foods = JsonConvert.DeserializeObject<SearchBase>(results);
            }
            else
            {
                Console.WriteLine("Error calling web API");
            }

            //ViewData.Model = foods;
        }

        return View(foods);
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
}

