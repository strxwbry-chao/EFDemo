using Microsoft.EntityFrameworkCore;
using EFDemo.Infrastructure.Data;
using EFDemo.Domain.Repositories;
using EFDemo.Infrastructure.Repositories;
using EFDemo.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework with SQLite
builder.Services.AddDbContext<CustomerContext>(options =>
{
    options.UseSqlite("Data Source=customers.db");
    
    // Enable detailed logging in development only
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Register Repository pattern services (Scoped lifetime for database connections)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Register Service layer
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Register API controllers and OpenAPI documentation
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

// Ensure database is created and seeded (demo purposes only)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CustomerContext>();
    context.Database.EnsureCreated();
    await DatabaseSeeder.SeedAsync(context);
}

app.MapControllers();

app.Run();
