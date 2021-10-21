using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bank.Context;
using Bank.Models;

namespace Bank
{
    public class Program
    {
        static void Main(string[] args)
        {
            IClientDataAccess access = new ClientJsonAccess();
            List<Client> clients = access.GetAll();
            
            Console.WriteLine(" \r\n  Tous les clients : \r\n ----------------------------");
            
            foreach (Client obj in clients)
            {
                Console.WriteLine(obj);
            }

            
            Console.WriteLine("Update client 1 ");
            access.UpdateClient(new Client()
            {
                Firstname = "mon nouveau prenom",
                Lastname = "mon dfhqdsh prenom",
                Guid = 1
            });
            
            Console.WriteLine("\r\n  Tous les clients : \r\n ----------------------------");
            
            clients = access.GetAll();
            foreach (Client obj in clients)
            {
                Console.WriteLine(obj);
            }
            
            Console.WriteLine("Delete client 5 ");
            access.DeleteClient(5);
            
            Console.WriteLine("\r\n Tous les clients : \r\n ----------------------------");
            
            clients = access.GetAll();
            foreach (Client obj in clients)
            {
                Console.WriteLine(obj);
            }

        }

    }
}