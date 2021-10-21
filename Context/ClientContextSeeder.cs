using System.Collections.Generic;
using System.Linq;
using Bank.Models;

namespace Bank.Context
{
    public class ClientContextSeeder
    {
        public static void Seed(ClientDbContext context)
        {
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            Currency usdFromDb = new Currency() {Name = "USD"};
            Currency eurFromDb = new Currency() {Name = "EUR"};
            //Currency eurFromDb = context.Currencies.Where(c => c.Name == "EUR").First();
            //Currency usdFromDb = context.Currencies.Where(c => c.Name == "USD").First();
            Client a = new Client() {Firstname = "test", Lastname = "lenaerts"};
            Client f1 = new Client() {Firstname = "resr", Lastname = "l1"};
            Client f2 = new Client() {Firstname = "netestw", Lastname = "l2",};
            Client f3 = new Client() {Firstname = "testset", Lastname = "l3"};

            CurrencyClient a1 = new CurrencyClient() {Client = a, Currency = eurFromDb, HasMain = true};
            CurrencyClient a2 = new CurrencyClient() {Client = f1, Currency = eurFromDb, HasMain = false};
            CurrencyClient a3 = new CurrencyClient() {Client = f2, Currency = eurFromDb, HasMain = true};
            CurrencyClient a4 = new CurrencyClient() {Client = f3, Currency = eurFromDb, HasMain = false};

            CurrencyClient a5 = new CurrencyClient() {Client = a, Currency = usdFromDb, HasMain = false};
            CurrencyClient a6 = new CurrencyClient() {Client = f1, Currency = usdFromDb, HasMain = true};
            CurrencyClient a7 = new CurrencyClient() {Client = f2, Currency = usdFromDb, HasMain = false};
            CurrencyClient a8 = new CurrencyClient() {Client = f3, Currency = usdFromDb, HasMain = true};
            
            context.CurrenciesClients.AddRange(new List<CurrencyClient>() {a1,a2,a3,a4,a5,a6,a7,a8});
            
            context.Clients.AddRange(new List<Client>() {a, f1, f2, f3});



            context.SaveChanges();
        }
    }
}