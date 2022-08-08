using BusRouteApi.DatabaseLayer;
using BusRouteApi.Helpers;
using BusRouteApi.JwtMiddleware;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddCors(options => options.AddPolicy("general", builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddDbContext<BusRouteDbContext>(option => option.UseNpgsql(configuration.GetSection("AppSettings:Database").Value));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    var security = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Please enter token",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };


    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BusRouteAPI", Version = "v1" });
    c.AddSecurityDefinition(security.Reference.Id, security);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            security, Array.Empty<string>()
        }
    });

});

builder.Services.AddTransient(typeof(BusRepository), typeof(BusRepository));
builder.Services.AddTransient(typeof(BusRouteRepository), typeof(BusRouteRepository));
builder.Services.AddTransient(typeof(OilPriceRepository), typeof(OilPriceRepository));
builder.Services.AddTransient(typeof(PayeeRepository), typeof(PayeeRepository));
builder.Services.AddTransient(typeof(RoutePriceRepository), typeof(RoutePriceRepository));
builder.Services.AddTransient(typeof(RouteRepository), typeof(RouteRepository));
builder.Services.AddTransient(typeof(ShiftRepository), typeof(ShiftRepository));
builder.Services.AddTransient(typeof(UserRepository), typeof(UserRepository));
builder.Services.AddTransient(typeof(VendorPayeeRepository), typeof(VendorPayeeRepository));
builder.Services.AddTransient(typeof(VendorRepository), typeof(VendorRepository));
builder.Services.AddTransient(typeof(SummaryRepository), typeof(SummaryRepository));

builder.Services.AddTransient(typeof(BusRouteService), typeof(BusRouteService));
builder.Services.AddTransient(typeof(BusService), typeof(BusService));
builder.Services.AddTransient(typeof(OilPriceService), typeof(OilPriceService));
builder.Services.AddTransient(typeof(PayeeService), typeof(PayeeService));
builder.Services.AddTransient(typeof(RoutePriceService), typeof(RoutePriceService));
builder.Services.AddTransient(typeof(RouteService), typeof(RouteService));
builder.Services.AddTransient(typeof(ShiftService), typeof(ShiftService));
builder.Services.AddTransient(typeof(UserService), typeof(UserService));
builder.Services.AddTransient(typeof(VendorPayeeService), typeof(VendorPayeeService));
builder.Services.AddTransient(typeof(VendorService), typeof(VendorService));
builder.Services.AddTransient(typeof(SummaryService), typeof(SummaryService));

builder.Services.AddTransient(typeof(BusRouteDbContext), typeof(BusRouteDbContext));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseRouting();

app.UseCors("general");

app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
