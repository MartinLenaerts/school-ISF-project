using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bank.Context;

namespace Bank.Models
{
    public class ClientDBAccess : IClientDataAccess
    {
        public static ClientDbContext context = new ClientDbContext();

        public List<Client> GetAll()
        {
            return context.Clients.ToList();
        }

        public bool CreateUser()
        {
            throw new System.NotImplementedException();
        }

        public Client GetClient(int guid)
        {
            return context.Clients.FirstOrDefault(c => c.Guid == guid);
        }

        public bool UpdateClient(Client c)
        {
            try
            {
                Client client = context.Clients.FirstOrDefault(c => c.Guid == c.Guid);
                client.merge(c);
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool DeleteClient(int guid)
        {
            try
            {
                var client = context.Clients.First();
                context.Clients.Remove(client);
                context.SaveChanges();
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