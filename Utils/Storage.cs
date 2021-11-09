using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Bank.Access;
using Bank.Context;

namespace Bank
{
    public class Storage
    {
        public Storage()
        {
            try
            {
                DataAccess = new ClientDbAccess();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                try
                {
                    DataAccess = new ClientJsonAccess();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        public Storage(bool forceJson)
        {
            if (forceJson)
                try
                {
                    DataAccess = new ClientJsonAccess();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            else
                try
                {
                    DataAccess = new ClientDbAccess();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    try
                    {
                        DataAccess = new ClientJsonAccess();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                }
        }

        public IClientDataAccess DataAccess { get; set; }

        public bool Synchronize()
        {
            Console.WriteLine("Synchronization ...");
            try
            {
                if (DataAccess.GetType().Name == "ClientDbAccess")
                {
                    Console.WriteLine("SQLite used ...");
                    Console.WriteLine("JSON update ...");
                    var context = new ClientJsonContext
                    {
                        Clients = ClientDbAccess.Context.Clients.ToList(),
                        Currencies = ClientDbAccess.Context.Currencies.ToList(),
                        Transactions = ClientDbAccess.Context.Transactions.ToList(),
                        CurrenciesClients = ClientDbAccess.Context.CurrenciesClients.ToList()
                    };
                    var jsonString = JsonSerializer.Serialize(context);
                    File.WriteAllText(ClientJsonAccess.DbJsonFile, jsonString);
                }
                else if (DataAccess.GetType().Name == "ClientJsonAccess")
                {
                    Console.WriteLine("JSON used ...");
                    Console.WriteLine("SQLite update ...");
                    var context = new ClientDbContext();
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    var actualContext = ((ClientJsonAccess) DataAccess).GetContext();
                    context.Clients.AddRange(actualContext.Clients);
                    context.Currencies.AddRange(actualContext.Currencies);
                    context.Transactions.AddRange(actualContext.Transactions);
                    context.CurrenciesClients.AddRange(actualContext.CurrenciesClients);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Synchronization Error ! ");
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Synchronization Completed");
            Console.ResetColor();
            return true;
        }

        public string GetStorageType()
        {
            try
            {
                if (DataAccess.GetType().Name == "ClientDbAccess") return "SQLite storage";
                if (DataAccess.GetType().Name == "ClientJsonAccess") return "JSON storage";
                return "#Error#";
            }
            catch (Exception e)
            {
                return "#Error#" + e.Message;
            }
        }
    }
}