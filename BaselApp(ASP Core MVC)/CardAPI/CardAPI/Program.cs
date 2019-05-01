using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaselFinalProjectApp.Models;
using CardAPI.DAL;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CardAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost webHost = CreateWebHostBuilder(args).Build();

            using (IServiceScope serviceScope = webHost.Services.CreateScope())
            {
                using(CardDbContext cardDb = serviceScope.ServiceProvider.GetRequiredService<CardDbContext>())
                {
                    if (!cardDb.Cards.Any())
                    {
                        Card Card1 = new Card()
                        {
                            Number = "5678123491234567",
                            ValidTHRU = "17/08",
                            CVV = "179",
                            AddedDate = DateTime.Now,
                            Balance = 400,
                            IsUsed = true
                        };

                        Card Card2 = new Card()
                        {
                            Number = "1234567891234567",
                            ValidTHRU = "15/08",
                            CVV = "178",
                            AddedDate = DateTime.Now,
                            Balance = 800,
                            IsUsed = true
                        };

                        Card Card3 = new Card()
                        {
                            Number = "9123123456784567",
                            ValidTHRU = "03/08",
                            CVV = "158",
                            AddedDate = DateTime.Now,
                            Balance = 500,
                            IsUsed = true
                        };

                        cardDb.Cards.AddRange(Card1,Card2,Card3);

                        cardDb.SaveChanges();
                    }
                }
            }

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
