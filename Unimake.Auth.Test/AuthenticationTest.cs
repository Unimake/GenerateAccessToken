using System.Text;

namespace Unimake.Auth.Test
{
    public class AuthenticationTest
    {
        #region Public Methods

        [Fact]
        public async Task Authenticate()
        {
            // Cria uma instância do HttpClient
            using(var client = new HttpClient())
            {
                // Define a URL da API
                var url = "https://unimake.app/auth/api/auth";

                // Cria o conteúdo da requisição no formato JSON
                var jsonContent = "{\"appId\": \"<<SEU APPID>>\",\"secret\": \"<<SEU SECRET>>\"}";
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Configura os cabeçalhos da requisição
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                try
                {
                    // Envia a requisição POST
                    var response = await client.PostAsync(url, content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    // Verifica se a resposta foi bem-sucedida
                    if(response.IsSuccessStatusCode)
                    {
                        // Ler o conteúdo da resposta
                        Console.WriteLine("Resposta da API:");
                        Console.WriteLine(responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"Falha na requisição: {response.StatusCode}{Environment.NewLine}{responseBody}");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
                }
            }
        }

        #endregion Public Methods
    }
}