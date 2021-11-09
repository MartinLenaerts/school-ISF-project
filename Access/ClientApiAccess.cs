using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Bank.Utils;

namespace Bank.Access
{
    public class ClientApiAccess
    {
        private static readonly HttpClient Client = new();
        private readonly string _apiKey = Environment.GetEnvironmentVariable("API_KEY");


        private string GetUrl()
        {
            return "https://v6.exchangerate-api.com/v6/" + _apiKey + "/";
        }


        public async Task<double> GetPair(string baseCode, string targetCode)
        {
            try
            {
                var url = GetUrl() + "pair/" + baseCode + "/" + targetCode;
                var res = JsonSerializer.Deserialize<ApiRes>(await Client.GetStringAsync(url));
                if (res.result != "success") throw new Exception("Error API");
                return res.conversion_rate;
            }
            catch (Exception e)
            {
                CustomConsole.Print(e.Message);
                CustomConsole.PrintError(
                    "An error occured, check your internet connection and check your api key in the environment file");
                throw;
            }
        }
    }
}