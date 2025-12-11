using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using VetCalendar.Application.Abstractions;
using VetCalendar.Application.CreateClient;
using VetCalendar.Application.GetClients;

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

        group.MapGet("/", async (IQueryHandler<GetClientsQuery, IReadOnlyList<ClientDto>> handler, 
            CancellationToken ct) =>
        {
            var result = await handler.Handle(new GetClientsQuery(), ct);

            return result.ToOkHttpResult();
        });

        return app;
    }


    public sealed record CreateClientRequest(
        [property: DefaultValue("John")]
        string FirstName,
        [property: DefaultValue("Doe")]
        string LastName,
        [property: DefaultValue("john.doe@example.com")]
        string Email,
        [property: DefaultValue("+33601020304")]
        string PhoneNumber);
}