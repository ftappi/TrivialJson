using System.Text.Json;
using TrivialJson.Models;

namespace TrivialJson
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string url = "https://opentdb.com/api.php?amount=10&category=18&type=multiple";

            HttpClient httpClient = new HttpClient();

            string risposta = httpClient.GetStringAsync(url).Result;

            Root r = JsonSerializer.Deserialize<Root>(risposta);
        }
    }
}
