dotnet ef dbcontext scaffold "YourConnectionString" Microsoft.EntityFrameworkCore.SqlServer --table table1,table2 --output-dir Models


    [Required]
    [Column(TypeName = "bigint")]
    [Index(IsUnique = true)]
    public long ContractorID { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Contractor>()
        .HasIndex(c => c.ContractorID)
        .IsUnique();

    modelBuilder.Entity<Contractor>()
        .Property(c => c.ContractorID)
        .IsRequired();
}


        payload.presentplans = payload.presentplans
            .Select(plan => new Plan
            {
                rates = Math.Round(plan.rates, 2),
                Name = plan.Name
            })
            .ToList();




{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    }
}
