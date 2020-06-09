using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Repository.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug() //设置输出日志的最小级别
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)//命名空间以Microsoft开头的日志输出的最小级别设置为Information
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .WriteTo.File(Path.Combine("logs", "api.txt"), rollingInterval: RollingInterval.Day)
               .CreateLogger();

             CreateWebHostBuilder(args).Build().Run();
     


        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)//
                .UseStartup<Startup>().UseSerilog();//覆盖原有的log操作
    }
}