using System;
using System.Collections.Generic;
using Bank.Access;
using Bank.Context;
using Bank.Models;
using Bank.Utils;

namespace Bank
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*ClientContextSeeder.Seed(ClientDbAccess.Context);
            Client client = new Storage().DataAccess.GetClient(1);
            Console.WriteLine(client);*/
            App app = new App();
            app.Start();

            app.Storage.Synchronize();
        }
    }
}