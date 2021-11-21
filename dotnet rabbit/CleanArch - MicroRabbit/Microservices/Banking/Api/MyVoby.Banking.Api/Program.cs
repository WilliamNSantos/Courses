using MyVoby.Infra.IoC;
using MyVoby.Banking.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BankingDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("BankingDbConnection"));
});

DependencyContainer.RegisterServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(/*c =>
    {
        c.SwaggerDoc("v1", new Info { Title = "Banking Microservices", Version = "v1"});
    }*/
);

builder.Services.AddMediatR(typeof(Startup));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Microservices v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
