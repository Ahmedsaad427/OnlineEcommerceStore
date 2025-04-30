
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistence;
using Presistence.Data;
using Services;
using Services.Abstractions;
using Shared.ErrorModels;
using Store.G03.API.Extentions;
using Store.G03.API.Middlewares;
using System.Threading.Tasks;

namespace Store.G03.API
    {
    public class Program
        {
        public static async Task Main(string[] args)
            {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewares();

            app.Run();
            }
        }
    }
