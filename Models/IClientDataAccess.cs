using System.Collections.Generic;

namespace Bank.Models
{
    public interface IClientDataAccess
    {
        public List<Client> GetAll();
        public bool CreateUser();
        public Client GetClient(int guid);
        public bool UpdateClient(Client c);
        public bool DeleteClient(int guid);
    }
}