
using ETL_test.DbContexts;
using ETL_test.Repository.ObjectModelRep;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ETL_test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(opt => opt.AddPolicy("AllowAnyOrigin", 
            builder =>{ builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));

            builder.Services.AddSwaggerGen(c => c.EnableAnnotations());
            builder.Services.AddControllers();
            builder.Services.AddDbContext<ObjectModelDbContext>(opt => 
                opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL_CS")));
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped(typeof(IObjectModelRepo), typeof(ObjectModelRepo));

            var app = builder.Build();
            app.UseCors("AllowAnyOrigin");

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ObjectModelDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
