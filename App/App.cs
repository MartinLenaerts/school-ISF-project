using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Access;
using Bank.Context;
using Bank.Models;
using Bank.Utils;

namespace Bank
{
    public class App
    {
        public Storage Storage;

        public App(bool forceJson = false)
        {
            Storage = new Storage(forceJson);
            SeedAsk:
            CustomConsole.PrintInfo(
                "Do you want to insert new data in database ? " +
                "Warning ! This action will remove all old data (y,N) ",
                false);
            var answer = Console.ReadLine();
            switch (answer.ToLower())
            {
                case "y":
                    ClientContextSeeder.Seed(ClientDbAccess.Context);
                    break;
                case "n":
                    break;
                case "":
                    break;
                default:
                    goto SeedAsk;
            }
        }

        public Admin Admin { get; set; }
        public Client ClientCurrent { get; set; }


        public async Task<bool> Start()
        {
            var stop = false;
            while (!stop)
            {
                Begin:
                Console.Clear();
                PrintWelcomeMessage();
                var key = Console.ReadLine();
                Console.WriteLine("");

                if (key == "1") // Is Admin
                {
                    stop = new AdminApp(Storage).Start();
                }
                else if (key == "2") // Is Client
                {
                    stop = await new ClientApp(Storage).Start();
                }
                else if (key == "q" || key == "Q") // Quit
                {
                    stop = true;
                }
                else
                {
                    CustomConsole.PrintError("Veuillez entrer un caractère valide [1,2 ou Q] ");
                    goto Begin;
                }
            }

            return stop;
        }


        public void PrintWelcomeMessage()
        {
            
            CustomConsole.PrintStyleInfo("Welcome to The Bank");
            CustomConsole.PrintInfo("Enter : ");
            var choices = new List<Choice>
            {
                new() {Key = "1", Message = "if you are an Admin"},
                new() {Key = "2", Message = "if you are a Client"},
                new() {Key = "Q", Message = "to quit"}
            };
            CustomConsole.PrintAllChoices(choices);
        }
    }
}