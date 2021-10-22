using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Bank.Access;
using Bank.Context;
using Bank.Models;

namespace Bank
{
    public class Storage
    {
        public IClientDataAccess DataAccess { get; set; }

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
            {
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
            else
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
        }

        public bool Synchronize()
        {
            Console.WriteLine("Synchronization ...");
            try
            {
                if (DataAccess.GetType().Name == "ClientDbAccess")
                {
                    Console.WriteLine("SQLite used ...");
                    Console.WriteLine("JSON update ...");
                    ClientJsonContext context = new ClientJsonContext()
                    {
                        Clients = ClientDbAccess.Context.Clients.ToList(),
                        Currencies = ClientDbAccess.Context.Currencies.ToList(),
                        Transactions = ClientDbAccess.Context.Transactions.ToList(),
                        CurrenciesClients = ClientDbAccess.Context.CurrenciesClients.ToList()
                    };
                    Console.WriteLine(context);
                    string jsonString = JsonSerializer.Serialize<ClientJsonContext>(context);
                    File.WriteAllText(ClientJsonAccess.DbJsonFile, jsonString);
                }
                else if (DataAccess.GetType().Name == "ClientJsonAccess")
                {
                    Console.WriteLine("JSON used ...");
                    Console.WriteLine("SQLite update ...");
                    ClientDbContext context = new ClientDbContext();
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    ClientJsonContext actualContext = ((ClientJsonAccess) DataAccess).GetContext();
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

                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Synchronization Completed");
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