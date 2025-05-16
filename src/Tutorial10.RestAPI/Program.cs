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

app.MapGet("/api/departments", async (SimpleCompanyContext context, CancellationToken cancellationToken) => {
    try
    {
        return Results.Ok(await context.Departemnts.ToListAsync(cancellationToken));
    }
    catch(Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/employees", async (SimpleCompanyContext context, CancellationToken cancellationToken) => {
    try
    {
        return Results.Ok(await context.Employees.ToListAsync(cancellationToken));
    }
    catch
    {
        return Results.Problem();
    }
});

app.MapGet("/api/employees/{id}", async (int id, SimpleCompanyContext context, CancellationToken cancellationToken) =>
{
    try
    {
        var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return Results.Ok(employee);
    }
    catch
    {
        return Results.Problem();
    }
});

app.MapPost("/api/employees", async (Employee employee, SimpleCompanyContext context, CancellationToken cancellationToken) =>
{
    try
    {
        context.Employees.Add(employee);
        await context.SaveChangesAsync(cancellationToken);
        return Results.Created($"/api/employees/{employee.Id}", employee);
    }
    catch
    {
        return Results.Problem();
    }
});

app.MapPut("/api/employees/{id}", async (int id, Employee updatedEmp, SimpleCompanyContext context, CancellationToken cancellationToken) =>
{
    try
    {
        var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (employee == null) return Results.NotFound();
        
        employee.Name = updatedEmp.Name;
        employee.JobId = updatedEmp.JobId;
        employee.ManagerId = updatedEmp.ManagerId; 
        employee.HireDate = updatedEmp.HireDate;
        employee.Salary = updatedEmp.Salary;
        employee.Commission = updatedEmp.Commission;
        employee.DepartmentId = updatedEmp.DepartmentId;
        employee.InverseManager = updatedEmp.InverseManager;
        employee.Job = updatedEmp.Job;
        employee.Manager = updatedEmp.Manager;
        
        
        await context.SaveChangesAsync(cancellationToken);
        return Results.Ok(employee);
    }
    catch
    {
        return Results.Problem();
    }
    
});

app.MapDelete("/api/employees/{id}", async (int id, SimpleCompanyContext context, CancellationToken cancellationToken) =>
{
    try
    {
        var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        context.Employees.Remove(employee);
        return Results.Ok(employee);
    }
    catch
    {
        return Results.Problem();
    }
});

app.Run();
