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
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string databasePath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}database.db";
            options.UseSqlite($"Data Source={databasePath}");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyClient>()
                .HasKey(um => um.CurrencyClientId);
            
            modelBuilder.Entity<CurrencyClient>()
                .HasOne(um => um.Client).WithMany(g => g.CurrencyClients)
                .HasForeignKey(um => um.ClientId);

            modelBuilder.Entity<CurrencyClient>()
                .HasOne(um => um.Currency).WithMany(g => g.CurrencyClients)
                .HasForeignKey(um => um.CurrencyId);

            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);
            
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Receiver).WithMany(c => c.TransactionsReceiver)
                .HasForeignKey(t => t.ReceiverId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Sender).WithMany(g => g.TransactionsSender)
                .HasForeignKey(t => t.SenderId);
        }
    }
}