var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireCsharpFunctional_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");



builder.Build().Run();
