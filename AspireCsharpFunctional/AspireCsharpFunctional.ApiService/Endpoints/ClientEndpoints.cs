using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using VetCalendar.Application.Abstractions;
using VetCalendar.Application.CreateClient;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
namespace AspireCsharpFunctional.ApiService.Endpoints;

public static class ClientEndpoints
{
    public static IEndpointRouteBuilder MapClientEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/clients");

        group.MapPost("/", async (
            CreateClientRequest request,
            ICommandHandler<CreateClientCommand> handler,
            CancellationToken ct) =>
        {
            var command = new CreateClientCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber);

            Result result = await handler.Handle(command, ct);

            return result.ToNoContentHttpResult();
        });

        return app;
    }


    public sealed record CreateClientRequest(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber);
}