using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExampleAPI.Models;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using ExampleAPI.Services;
using System.ComponentModel.DataAnnotations;
using ExampleAPI.Constants;

namespace ExampleAPI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IApiService _apiService;
    private readonly IConfiguration _config;

    string baseUrl = "https://api.nal.usda.gov/fdc/v1/";
    string dataType = "Foundation";
    string pageSize = "20";

    public HomeController(ILogger<HomeController> logger, IConfiguration config, IApiService apiService)
    {
        _logger = logger;
        _config = config;
        _apiService = apiService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> SearchByQuery(string query)
    {
        SearchBase foods = new SearchBase();
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(QueryParams.BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage getData = await client.GetAsync(_apiService.GetDataBySearchQuery(query));

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

