using ToDosProject.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var sqldb = builder.AddConnectionString(AppConfiguration.SQL, "ConnectionStrings__SqlServer");

var apiService = builder.AddProject<Projects.ToDosProject_ApiService>(AppConfiguration.API)
    .WithReference(sqldb);

builder.AddMobileProject("mauiclient", "../ToDosProject.App")
    .WithReference(apiService);

builder.AddProject<Projects.ToDosProject_Web>(AppConfiguration.WEB)
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
