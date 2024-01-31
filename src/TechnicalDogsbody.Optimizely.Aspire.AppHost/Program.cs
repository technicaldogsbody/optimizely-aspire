var builder = DistributedApplication.CreateBuilder(args);

builder.AddSqlServerContainer("EPiServerDB", "0pti_R0cks", 1433)
        .WithVolumeMount("./App_Data/database-files", "/var/opt/mssql/data", VolumeMountType.Bind);

builder.AddProject<Projects.TechnicalDogsbody_Optimizely_Aspire_Web>("technicaldogsbody.optimizely.aspire.web")
    .WithEnvironment("RUN_MODE", Environment.GetEnvironmentVariable("RUN_MODE"));

builder.Build().Run();
