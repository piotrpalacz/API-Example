using System;
using ExampleAPI.Models;

namespace ExampleAPI.Services
{
    public interface IApiService
    {
       string GetDataBySearchQuery(string query);
    }
}

