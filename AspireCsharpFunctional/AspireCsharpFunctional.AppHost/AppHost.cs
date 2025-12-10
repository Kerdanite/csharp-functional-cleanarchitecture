using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);



var sql = builder.AddSqlServer("sqlserver")
    .WithDataVolume();
var apiDb = sql.AddDatabase("apidb");


var apiService = builder.AddProject<Projects.AspireCsharpFunctional_ApiService>("apiservice")
    .WithReference(apiDb)          // << lien vers la DB
    .WaitFor(apiDb)
    .WithHttpHealthCheck("/health")
    .WithUrlForEndpoint("http", url =>
    {
        url.DisplayText = "Swagger";
        url.Url = "/swagger";
    });



builder.Build().Run();
