using System;
using Bank.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Context
{
    public class ClientDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyClient> CurrenciesClients { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        
        public DbSet<Message> Messages { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var databasePath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}database.db";
            options.UseSqlite($"Data Source={databasePath}");
        }
    }
}