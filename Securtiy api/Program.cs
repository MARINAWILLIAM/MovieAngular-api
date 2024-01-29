using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Core;
using Security.Core.SecurityEntites;
using Security.Repo.Data.Identity;
using Security.Repo.SecurityEntites;
using Securityservices;
using Securtiy_api.Errors;
using System.Text;

namespace Securtiy_api
{
    public class Program
    {
        public static async Task Main(string[] args)

        {
            IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //stepthree
            builder.Services.AddDbContext<AppSecurityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SecurityConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppSecurityDbContext>()
                ;
            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(
                options=>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                    ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidationIssure"],
                    ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidationAudience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"])),

                };
                });
            builder.Services.AddScoped<ItokenServices, TokenServices>();
            builder.Services.Configure<ApiBehaviorOptions>(Options =>
            {
                //factory               response mas2ol     context action eli bytnfaz fe lahza deh
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    //in it kol haga t5os   eli valid halayn
                    //creat objApiValdtionErrorResponse
                    //dictionary array of error llparmeter key ==nameofpar
                                                    //maskt par 3andha error not valid state 
                    var errors= actionContext.ModelState.Where(p=>p.Value.Errors.Count()>0)
                    .SelectMany(p=>p.Value.Errors)
                    .Select(E=>E.ErrorMessage)
                    .ToArray();
                    var validationErrorResponse = new ApiValdtionErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            var app = builder.Build();
            //seed create dbcontext
            //createScope
            // var scope = app.Services;//service koloha
            using var scope = app.Services.CreateScope();//services sh8la scope bas
            var services = scope.ServiceProvider;
            var loggerFactory=services.GetRequiredService<ILoggerFactory>();
            try
            {
               
                var dbcontext = services.GetRequiredService<AppSecurityDbContext>();
                //ask clr for creating object from dbcontext 
                await dbcontext.Database.MigrateAsync();
                var usermanager = services.GetRequiredService<UserManager<AppUser>>();
                await SecurtiyDbcontectSeed.SeedUsersAsync(usermanager);
            }
            catch (Exception ex)
            {
                var logger= loggerFactory.CreateLogger<Program>();
                logger.LogError(ex,"an error occured during apply the migration");
            }
          
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }
    }
}