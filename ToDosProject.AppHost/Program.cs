using ToDosProject.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis(AppConfiguration.CACHE);

var sqldb = builder.AddConnectionString(AppConfiguration.SQL, "ConnectionStrings__SqlServer");

var apiService = builder.AddProject<Projects.ToDosProject_ApiService>(AppConfiguration.API)
    .WithReference(sqldb);

builder.AddProject<Projects.ToDosProject_Web>(AppConfiguration.WEB)
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.AddProject<Projects.ToDosProject_MigrationService>(AppConfiguration.MIGRATION)
    .WithReference(sqldb);

builder.Build().Run();
