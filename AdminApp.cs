using System;
using System.Collections.Generic;
using Bank.Models;
using Bank.Utils;

namespace Bank
{
    public class AdminApp
    {
        public Admin Admin { get; set; }
        
        public Storage Storage { get; set; }
        public bool Start()
        {
            PrintWelcomeMessage();
            AskAdminCredentials:
                    if (!AskAdminCredentials()) // Ask Credentials
                    {
                        CustomConsole.PrintError("\r\n Username ou password incorrect");
                        goto AskAdminCredentials;
                    }

                    Admin = new Admin();
                    BeginAdmin:
                    PrintWelcomeMessage();
                    string key = Console.ReadLine();
                    switch (key)
                    {
                        case "1": // Create Client
                            createClient:
                            if (!CreateClient())
                            {
                                CustomConsole.PrintError("Une erreur s'est produite");
                                goto createClient;
                            }

                            CustomConsole.PrintSuccess("Client bien créé ! ");
                            goto BeginAdmin;
                        case "2": // Manage Client
                            if (!ManageClient())
                            {
                                CustomConsole.PrintError("Une erreur s'est produite");
                                goto BeginAdmin;
                            }

                            CustomConsole.PrintSuccess("Client modifié ! ");
                            goto BeginAdmin;
                        case "3": // Verify user transactions
                            VerifyTransactions:
                            if (!GetTransactions())
                            {
                                CustomConsole.PrintError("Une erreur s'est produite");
                                goto VerifyTransactions;
                            }

                            goto BeginAdmin;
                        case "4": // View all clients
                            if (!ViewAllClients())
                            {
                                CustomConsole.PrintError("Une erreur s'est produite");
                            }

                            goto BeginAdmin;
                        case "d": // Disconnect
                            Admin = null;
                            CustomConsole.PrintSuccess("Vous avez bien été déconnecté \r\n");
                            return false;
                        case "D": // Disconnect
                            Admin = null;
                            CustomConsole.PrintSuccess("Vous avez bien été déconnecté \r\n");
                            return false;
                        case "q": // Quit
                            return true;
                        case "Q": // Quit
                           return true;
                        default:
                            goto BeginAdmin;
                    }
        }


        public void PrintWelcomeMessage()
        {
            CustomConsole.PrintInfo("Welcome Admin");
            CustomConsole.PrintInfo("Enter : ");
            List<Choice> choices = new List<Choice>()
            {
                new() {Key = "1", Message = "to create a client"},
                new() {Key = "2", Message = "to manage client"},
                new() {Key = "3", Message = "to verify user transactions"},
                new() {Key = "4", Message = "to view a list of all clients"},
                new() {Key = "D", Message = "to disconnect"},
                new() {Key = "Q", Message = "to quit"},
            };
            CustomConsole.PrintAllChoices(choices);
        }
        
        
        
        public bool AskAdminCredentials()
        {
            Console.Write("Veuillez entrer votre nom d'utilsateur : ");
            string username = Console.ReadLine();

            Console.Write("Veuillez entrer votre mot de passe : ");
            string password = CustomConsole.EnterPassword();

            return username == Admin.username && password == Admin.password;
        }
        
        


        public bool CreateClient()
        {   
            if (Admin is null) return false;
            CustomConsole.Print("Veuillez entrer les informations du nouveau client : ");
            Console.Write("Nom : ");
            string lastname = Console.ReadLine();
            Console.Write("Prenom : ");
            string firstname = Console.ReadLine();
            if (lastname == "" || firstname == "") return false;
            int pin = new Random().Next(1000, 10000);
            return Storage.DataAccess.CreateClient(new Client()
                {Lastname = lastname, Firstname = firstname, Pin = pin});
        }

        public bool ManageClient()
        {
            if (Admin is null) return false;
            CustomConsole.Print("Veuillez entrer le guid d'un client : ");
            string guid = Console.ReadLine();
            if (guid == "") return false;
            try
            {
                Client client = Storage.DataAccess.GetClient(Int32.Parse(guid));
                PrintManageClientMessage(client);
                string key = Console.ReadLine();
                if (key == "1") return Storage.DataAccess.DeleteClient(client.Guid);
                if (key == "2") return UpdateClient(client);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        public void PrintManageClientMessage(Client c)
        {
            CustomConsole.Print("Client found ! : " + c);
            CustomConsole.Print("Enter : ");
            CustomConsole.Print("1 : to delete");
            CustomConsole.Print("2 : to update");
        }

        public bool UpdateClient(Client c)
        {
            CustomConsole.Print("Enter : ");
            CustomConsole.Print("1 : to unblock");
            CustomConsole.Print("2 : to block");
            CustomConsole.Print("3 : to change pin");
            CustomConsole.Print("4 : to reset tries");
            CustomConsole.Print("5 : to delete client");
            CustomConsole.Print("5 : to update client informations (firstname and lastname)");
            string key = Console.ReadLine();
            switch (key)
            {
                case "1":
                    c.Blocked = false;
                    return Storage.DataAccess.UpdateClient(c);
                case "2":
                    c.Blocked = true;
                    return Storage.DataAccess.UpdateClient(c);
                case "3":
                    EnterPin:
                    Console.Write("Could you enter the new pin please : ");
                    string stringPin = Console.ReadLine();
                    int pin;
                    if (!Int32.TryParse(stringPin, out pin) || pin > 9999 || pin < 1000)
                    {
                        Console.Write("Could you enter a correct pin please : ");
                        goto EnterPin;
                    }

                    c.Pin = pin;
                    return Storage.DataAccess.UpdateClient(c);
                case "4":
                    return Storage.DataAccess.DeleteClient(c.Guid);
                case "5":
                    CustomConsole.Print("You can update client informations (if you don't want please press Enter ) ");
                    Console.Write("Firstname : ");
                    string firstname = Console.ReadLine();

                    Console.Write("Lastname : ");
                    string lastname = Console.ReadLine();

                    if (lastname != "") c.Lastname = lastname;
                    if (firstname != "") c.Lastname = firstname;

                    return Storage.DataAccess.UpdateClient(c);
                default:
                    return false;
            }
        }

        public bool GetTransactions()
        {
            CustomConsole.Print("Enter : ");
            CustomConsole.Print("1 : to show all transactions");
            CustomConsole.Print("2 : to show transactions of one client");
            string key = Console.ReadLine();
            switch (key)
            {
                case "1":
                    try
                    {
                        List<Transaction> transactions = Storage.DataAccess.GetAllTransactions();
                        foreach (var transaction in transactions)
                        {
                            Console.WriteLine(transaction);
                        }

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }

                case "2":
                    EnterGuid:
                    Console.Write("Could you enter the guid of client : ");
                    string stringGuid = Console.ReadLine();
                    int guid;
                    if (!Int32.TryParse(stringGuid, out guid))
                    {
                        Console.Write("Could you enter a correct pin please : ");
                        goto EnterGuid;
                    }

                    try
                    {
                        List<Transaction> transactions = Storage.DataAccess.GetClientTransactions(guid);
                        foreach (var transaction in transactions)
                        {
                            Console.WriteLine(transaction);
                        }

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }
                default:
                    return false;
            }
        }

        public bool ViewAllClients()
        {
            try
            {
                List<Client> clients = Storage.DataAccess.GetAll();
                foreach (var client in clients)
                {
                    Console.WriteLine(client);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        

    }
}