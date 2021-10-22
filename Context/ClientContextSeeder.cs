using System;
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
            Random r = new Random();
            Client a = new Client() {Firstname = "Nicolas", Lastname = "Sarkosy",Pin = r.Next(1000, 10000)};
            Client f1 = new Client() {Firstname = "Francois", Lastname = "Hollande",Pin = r.Next(1000, 10000)};
            Client f2 = new Client() {Firstname = "Jacques", Lastname = "Chirac",Pin = r.Next(1000, 10000)};
            Client f3 = new Client() {Firstname = "Emmanuel", Lastname = "Macron",Pin = r.Next(1000, 10000)};

            CurrencyClient a1 = new CurrencyClient() {Client = a, Currency = eurFromDb, HasMain = true , Amount = 12000};
            CurrencyClient a2 = new CurrencyClient() {Client = f1, Currency = eurFromDb, HasMain = false , Amount = 2453};
            CurrencyClient a3 = new CurrencyClient() {Client = f2, Currency = eurFromDb, HasMain = true , Amount = 453};
            CurrencyClient a4 = new CurrencyClient() {Client = f3, Currency = eurFromDb, HasMain = false , Amount = 453};

            CurrencyClient a5 = new CurrencyClient() {Client = a, Currency = usdFromDb, HasMain = false , Amount = 7867812};
            CurrencyClient a6 = new CurrencyClient() {Client = f1, Currency = usdFromDb, HasMain = true , Amount = 7864};
            CurrencyClient a7 = new CurrencyClient() {Client = f2, Currency = usdFromDb, HasMain = false , Amount = 21137};
            CurrencyClient a8 = new CurrencyClient() {Client = f3, Currency = usdFromDb, HasMain = true, Amount = 78624};
            
            context.CurrenciesClients.AddRange(new List<CurrencyClient>() {a1,a2,a3,a4,a5,a6,a7,a8});
            
            context.Clients.AddRange(new List<Client>() {a, f1, f2, f3});



            context.SaveChanges();
        }
    }
}