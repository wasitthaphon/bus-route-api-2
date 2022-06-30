using BusRouteApi.DatabaseLayer;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.ServiceLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(BusRepository), typeof(BusRepository));
builder.Services.AddTransient(typeof(PayeeRepository), typeof(PayeeRepository));
builder.Services.AddTransient(typeof(BusService), typeof(BusService));
builder.Services.AddTransient(typeof(PayeeRepository), typeof(PayeeRepository));
builder.Services.AddTransient(typeof(PayeeService), typeof(PayeeService));
builder.Services.AddTransient(typeof(BusRouteDbContext), typeof(BusRouteDbContext));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
