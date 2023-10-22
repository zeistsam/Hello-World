public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        // Register the first DbContext and configure services
        builder.Services.AddDbContext<YourFirstDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("FirstDbContextConnectionString"));
        });

        // Automatically apply migrations for the first DbContext during startup
        using (var serviceScope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<YourFirstDbContext>();
            context.Database.Migrate();
        }

        // Register the second DbContext and configure services
        builder.Services.AddDbContext<YourSecondDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("SecondDbContextConnectionString"));
        });

        // Automatically apply migrations for the second DbContext during startup
        using (var serviceScope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<YourSecondDbContext>();
            context.Database.Migrate();
        }
    }
}

//Use migrations understanding
