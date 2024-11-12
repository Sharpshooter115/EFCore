using Microsoft.Extensions.Options;
using MMABooksEFClasses.Models;

namespace MMABOOKSRESTAP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // ADD CORS POLICY - IN A PRODUCTION APP LOCK THIS DOWN!
            builder.Services.AddCors(options => {
                options.AddDefaultPolicy(
            builder => {
                builder.AllowAnyOrigin()
            .WithMethods("POST", "PUT", "DELETE", "GET", "OPTIONS")
            .AllowAnyHeader();
            });
  });
  // ADDING THE DBCONTEXT TO THE SERVICE
  builder.Services.AddDbContext<MMABooksContext>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

           // app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
