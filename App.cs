using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Access;
using Bank.Context;
using Bank.Models;
using Bank.Utils;

namespace Bank
{
    public class App
    {
        public Admin Admin { get; set; }
        public Client ClientCurrent { get; set; }

        public Storage Storage;

        public App(bool forceJson = false)
        {
            Storage = new Storage(forceJson);
            SeedAsk:
            CustomConsole.PrintInfo(
                "Do you want to insert new data in database ? " +
                "Warning ! This action will remove all old data (y,N) ",
                false);
            string answer = Console.ReadLine();
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


        public void Start()
        {
            bool stop = false;
            while (!stop)
            {
                Begin:
                PrintWelcomeMessage();
                string key = Console.ReadLine();
                Console.WriteLine("");

                if (key == "1") // Is Admin
                {
                    stop = new AdminApp().Start();
                }
                else if (key == "2") // Is Client
                {
                    stop = new ClientApp().Start();
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
        }


        public void PrintWelcomeMessage()
        {
            CustomConsole.PrintInfo("Welcome to The Bank");
            CustomConsole.PrintInfo("Enter : ");
            List<Choice> choices = new List<Choice>()
            {
                new() {Key = "1", Message = "if you are an Admin"},
                new() {Key = "2", Message = "if you are a Client"},
                new() {Key = "Q", Message = "to quit"},
            };
            CustomConsole.PrintAllChoices(choices);
        }
        
        
    }
}