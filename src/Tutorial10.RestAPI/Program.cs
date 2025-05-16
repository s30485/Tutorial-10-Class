using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tutorial10.RestAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Default connection string not found");

builder.Services.AddDbContext<SimpleCompanyContext>(options => options.UseSqlServer(ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/jobs", async (SimpleCompanyContext context, CancellationToken cancellationToken) => {
    try
    {
        return Results.Ok(await context.Jobs.ToListAsync(cancellationToken));
    }
    catch(Exception ex)
    {
        return Results.Problem(ex.Message);
    }
    
});

app.MapGet("/api/departments", () => {
    
});

app.MapGet("/api/employees", () =>
{
    
});

app.MapGet("/api/employees/{id}", (int id) =>
{
    
});

app.MapPost("/api/employees", () =>
{
    
});

app.MapPut("/api/employees/{id}", (int id) =>
{
    
});

app.MapDelete("/api/employees/{id}", (int id) =>
{
    
});

app.Run();
