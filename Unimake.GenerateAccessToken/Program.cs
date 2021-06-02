using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Unimake.GenerateAccessToken.Models;
using Unimake.GenerateAccessToken.Services;

namespace Unimake.GenerateAccessToken
{
    internal class Program
    {
        #region Private Methods

        private static async Task Main()
        {
            Console.Title = "Generate Access Token Example";
            var service = new AuthenticationService();

            var response = await service.AuthAsync(new AuthRequest
            {
                AppId = "7492002e16144c3ab7aa60bf65bdf60f",
                Secret = "421390128b27421180c9e10c0b0b0af7"
            });

            var json = JsonConvert.SerializeObject(response, Formatting.Indented);

            Console.WriteLine(json);
            Console.ReadKey();
        }

        #endregion Private Methods
    }
}