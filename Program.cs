using System;
using Bank.Context;
using Bank.Models;

namespace Bank
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            bool stop = false;
            App app = new App();
            while (!stop)
            {
                Begin:
                app.PrintWelcomeMessage();
                string key = Console.ReadLine();
                Console.WriteLine("");

                if (key == "1") // Is Admin
                {
                    AskAdminCredentials:
                    if (!app.AskAdminCredentials()) // Ask Credentials
                    {
                        PrintError("\r\n Username ou password incorrect");
                        goto AskAdminCredentials;
                    }

                    app.Admin = new Admin();
                    BeginAdmin:
                    Console.WriteLine("\r\n");
                    app.PrintWelcomeAdminMessage();
                    key = Console.ReadLine();
                    switch (key)
                    {
                        case "1": // Create Client
                            createClient:
                            if (!app.CreateClient())
                            {
                                PrintError("Une erreur s'est produite");
                                goto createClient;
                            }

                            PrintSuccess("Client bien créé ! ");
                            goto BeginAdmin;
                        case "2": // Manage Client
                            if (!app.ManageClient())
                            {
                                PrintError("Une erreur s'est produite");
                                goto BeginAdmin;
                            }

                            PrintSuccess("Client modifié ! ");
                            goto BeginAdmin;
                        case "3": // Verify user transactions
                            VerifyTransactions:
                            if (!app.GetTransactions())
                            {
                                PrintError("Une erreur s'est produite");
                                goto VerifyTransactions;
                            }

                            goto BeginAdmin;
                        case "4": // View all clients
                            if (!app.ViewAllClients())
                            {
                                PrintError("Une erreur s'est produite");
                            }

                            goto BeginAdmin;
                        case "d": // Disconnect
                            app.Admin = null;
                            PrintSuccess("Vous avez bien été déconnecté \r\n");
                            goto Begin;
                        case "D": // Disconnect
                            app.Admin = null;
                            PrintSuccess("Vous avez bien été déconnecté \r\n");
                            goto Begin;
                        case "q": // Quit
                            stop = true;
                            break;
                        case "Q": // Quit
                            stop = true;
                            break;
                        default:
                            goto BeginAdmin;
                    }
                }
                else if (key == "2") // Is Client
                {
                    AskUserCredentials:
                    if (!app.AskUserCredentials()) // Ask Credentials
                    {
                        PrintError("\r\n Guid or pin incorrect");
                        goto AskUserCredentials;
                    }

                    BeginClient:
                    Console.WriteLine("\r\n");
                    app.PrintWelcomeClientMessage();
                    key = Console.ReadLine();
                    switch (key)
                    {
                        case "1": //  view GUID and credentials
                            Console.WriteLine(app.ClientCurrent);
                            goto BeginClient;
                        case "2": // total amount in preferred currency
                            if (!app.ViewTotalAmount()) // Ask Credentials
                            {
                                PrintError("\r\n An eroor occured");
                            }
                            goto BeginClient;
                        case "3": // retrieve money from currency
                            RetrieveMoney:
                            if (!app.RetrieveMoney())
                            {
                                PrintError("\r\n An eroor occured");
                                goto RetrieveMoney;
                            }

                            goto BeginClient;
                        case "4": // add money from currency
                            AddMoney:
                            if (!app.AddMoney())
                            {
                                PrintError("\r\n An eroor occured");
                                goto AddMoney;
                            }

                            goto BeginClient;
                        case "5": // exchange money
                            ExchangeMoney:
                            if (!app.ExchangeMoney())
                            {
                                PrintError("\r\n An eroor occured");
                                goto ExchangeMoney;
                            }

                            goto BeginClient;
                        case "6": // transfert money
                            TransfertMoney:
                            if (!app.TransfertMoney())
                            {
                                PrintError("\r\n An eroor occured");
                                goto TransfertMoney;
                            }
                            goto BeginClient;
                        case "7": // change pin
                            ChangePin:
                            if (!app.ChangePin())
                            {
                                PrintError("\r\n An eroor occured");
                                goto ChangePin;
                            }
                            goto BeginClient;
                        case "8": // Leave message for admin
                            LeaveMsg:
                            if (!app.LeaveMsg())
                            {
                                PrintError("\r\n An eroor occured");
                                goto LeaveMsg;
                            }
                            goto BeginClient;
                        case "d": // Disconnect
                            app.ClientCurrent = null;
                            PrintSuccess("Vous avez bien été déconnecté \r\n");
                            goto Begin;
                        case "D": // Disconnect
                            app.ClientCurrent = null;
                            PrintSuccess("Vous avez bien été déconnecté \r\n");
                            goto Begin;
                        case "q": // Quit
                            stop = true;
                            break;
                        case "Q": // Quit
                            stop = true;
                            break;
                        default:
                            goto BeginClient;
                    }
                }
                else if (key == "q" || key == "Q") // Quit
                {
                    stop = true;
                }
                else
                {
                    PrintError("Veuillez entrer un caractère valide [1,2 ou Q] ");
                    goto Begin;
                }
            }

            app.Storage.Synchronize();
        }


        public static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void PrintSuccess(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}