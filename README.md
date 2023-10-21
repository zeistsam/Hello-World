Test 1

dotnet ef dbcontext scaffold "YourConnectionString" Microsoft.EntityFrameworkCore.SqlServer --table table1,table2 --output-dir Models

Scaffold-DbContext "YourConnectionString" Microsoft.EntityFrameworkCore.SqlServer -Tables table1,table2 -OutputDir Models

catch (DbUpdateException e) {
    var sqlException = e.GetBaseException() as SqlException;
    if (sqlException != null) {
        if (sqlException .Errors.Count > 0) {
            switch (sqlException .Errors[0].Number) {
                case 547: // Foreign Key violation
                    ModelState.AddModelError("CodeInUse", "Country code could not be deleted, because it is in use");
                    return View(viewModel.First());
                default: 
                    throw;      
            }
        }
    }

    

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
