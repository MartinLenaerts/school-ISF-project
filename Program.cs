using System.Threading.Tasks;

namespace Bank
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            /*ClientContextSeeder.Seed(ClientDbAccess.Context);
            Client client = new Storage().DataAccess.GetClient(1);
            Console.WriteLine(client);*/
            var app = new App();
            await app.Start();

            app.Storage.Synchronize();
        }
    }
}