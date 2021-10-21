using System;
using Bank.Models;

namespace Bank
{
    public class Storage
    {
        private IClientDataAccess DataAccess { get; set; }

        Storage()
        {
        }

        public bool Synchronize()
        {
            try
            {
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}