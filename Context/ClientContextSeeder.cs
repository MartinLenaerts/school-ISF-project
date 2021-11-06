using System;
using System.Collections.Generic;
using System.IO;
using Bank.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Context
{
    public class ClientContextSeeder
    {
        public static async void Seed(ClientDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var usdFromDb = new Currency {Name = "USD"};
            var eurFromDb = new Currency {Name = "EUR"};

            await context.Database.ExecuteSqlRawAsync(new StreamReader("../../../insertCurrency.sql").ReadToEnd());

            //Currency eurFromDb = context.Currencies.Where(c => c.Name == "EUR").First();
            //Currency usdFromDb = context.Currencies.Where(c => c.Name == "USD").First();
            var r = new Random();
            var a = new Client {Firstname = "Nicolas", Lastname = "Sarkosy", Pin = 1234};
            var f1 = new Client {Firstname = "Francois", Lastname = "Hollande", Pin = 1234};
            var f2 = new Client {Firstname = "Jacques", Lastname = "Chirac", Pin = 1234};
            var f3 = new Client {Firstname = "Emmanuel", Lastname = "Macron", Pin = 1234};

            var a1 = new CurrencyClient {Client = a, Currency = eurFromDb, HasMain = true, Amount = 12000};
            var a2 = new CurrencyClient {Client = f1, Currency = eurFromDb, HasMain = false, Amount = 2453};
            var a3 = new CurrencyClient {Client = f2, Currency = eurFromDb, HasMain = true, Amount = 453};
            var a4 = new CurrencyClient {Client = f3, Currency = eurFromDb, HasMain = false, Amount = 453};

            var a5 = new CurrencyClient {Client = a, Currency = usdFromDb, HasMain = false, Amount = 7867812};
            var a6 = new CurrencyClient {Client = f1, Currency = usdFromDb, HasMain = true, Amount = 7864};
            var a7 = new CurrencyClient {Client = f2, Currency = usdFromDb, HasMain = false, Amount = 21137};
            var a8 = new CurrencyClient {Client = f3, Currency = usdFromDb, HasMain = true, Amount = 78624};


            var t1 = new Transaction {Amount = 120, Receiver = a, Sender = f1};
            var t2 = new Transaction {Amount = 23, Receiver = a, Sender = f2};
            var t3 = new Transaction {Amount = 45, Receiver = f2, Sender = f3};

            context.Transactions.AddRange(new List<Transaction> {t1, t2, t3});

            context.CurrenciesClients.AddRange(new List<CurrencyClient> {a1, a2, a3, a4, a5, a6, a7, a8});

            context.Clients.AddRange(new List<Client> {a, f1, f2, f3});


            context.SaveChanges();
        }
    }
}