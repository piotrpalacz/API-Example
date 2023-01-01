using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using ExampleAPI.Constants;
using ExampleAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace ExampleAPI.Services
{
    public class ApiService : IApiService
    {
        private readonly IConfiguration _config;

        public ApiService(IConfiguration config)
        {
            _config = config;
        }

        public string GetDataBySearchQuery(string query)
        {
            var apiKey = _config["USDA:ApiKey"];

            return $"{QueryParams.BaseUrl}foods/search?query={query}&api_key={apiKey}&pageSize={QueryParams.PageSize}&dataType={QueryParams.DataType}";


        }
    }
}

