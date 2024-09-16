using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Digite a latitude: ");
        string latitude = Console.ReadLine();

        Console.Write("Digite a longitude: ");
        string longitude = Console.ReadLine();

        try
        {
            string respostaPrevisao = await ObterDadosPrevisao(latitude, longitude);
            Console.WriteLine(respostaPrevisao);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter dados da previsão do tempo: {ex.Message}");
        }
    }

    static async Task<string> ObterDadosPrevisao(string latitude, string longitude)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m";
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseBody);

                // Pegar as primeiras temperaturas da resposta
                JArray temperaturas = (JArray)json["hourly"]["temperature_2m"];

                string temperaturaInicial = temperaturas[0].ToString();
                string temperaturaSegunda = temperaturas[2].ToString();

                return $"Temperatura inicial: {temperaturaInicial}°C\nTemperatura da próxima hora: {temperaturaSegunda}°C";
            }
            else
            {
                throw new Exception("Não foi possível obter os dados da previsão do tempo.");
            }
        }
    }
}
