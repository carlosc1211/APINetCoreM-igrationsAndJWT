using Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ConnDbContext>(options => options.UseSqlServer(builder.Configuration["ConexionRepositorio"]));

builder.Services.AddCors(options =>
{
  options.AddPolicy("PoliticaCors",
               builder =>
               {
                 builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
               });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  ConnDbContext context = scope.ServiceProvider.GetRequiredService<ConnDbContext>();
  context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x
       .AllowAnyMethod()
       .AllowAnyHeader()
       .SetIsOriginAllowed(origin => true)); // allow any origin


app.UseAuthorization();

app.MapControllers();

app.Run();
