using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bank.Context;

namespace Bank.Models
{
    public class ClientJsonAccess : IClientDataAccess
    {
        private static string dbJsonFile = "database.json";

        private ClientJsonContext GetContext()
        {
            string dbJson = new StreamReader(dbJsonFile).ReadToEnd();
            ClientJsonContext json = JsonSerializer.Deserialize<ClientJsonContext>(dbJson);
            return json;
        }

        private bool PushContext(ClientJsonContext context)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize<ClientJsonContext>(context);
                File.WriteAllText(dbJsonFile, jsonString);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        public List<Client> GetAll()
        {
            ClientJsonContext context = GetContext();
            return context != null ? context.Clients : new List<Client>();
        }

        public bool CreateUser()
        {
            throw new System.NotImplementedException();
        }

        public Client GetClient(int guid)
        {
            ClientJsonContext context = GetContext();
            Client c = context.Clients.Find(c => c.Guid == guid);
            return c;
        }

        public bool UpdateClient(Client c)
        {
            try
            {
                ClientJsonContext context = GetContext();
                Client client = context.Clients.Find(client => client.Guid == c.Guid);
                client.merge(c);
                return PushContext(context);;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteClient(int guid)
        {
            try
            {
                ClientJsonContext context = GetContext();
                context.Clients.Remove(new Client() {Guid = guid});
                return PushContext(context);;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}