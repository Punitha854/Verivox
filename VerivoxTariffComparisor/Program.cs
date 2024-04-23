using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using VerivoxTariffComparisor.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<TariffDbContext>(
    option => option.UseInMemoryDatabase(builder.Configuration.GetConnectionString("TariffProviderDatabase")));
//builder.Services.AddScoped<TariffDbContext>();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TariffDbContext>();
    DataSeeder.Initialize(services);
}


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
