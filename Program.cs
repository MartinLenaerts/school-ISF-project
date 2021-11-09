using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Utils;

namespace Bank
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            DotEnv.Load("../../../.env.local");
            var app = new App();
            await app.Start();

            app.Storage.Synchronize();
        }
    }
}