using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using AspireCsharpFunctional.ApiService.Request;
using VetCalendar.Application.Abstractions;
using VetCalendar.Application.AddPatient;
using VetCalendar.Application.CreateClient;
using VetCalendar.Application.GetClientPatients;
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

        // POST /clients/{clientId}/patients
        group.MapPost("/{clientId:guid}/patients", async (
            Guid clientId,
            AddPatientRequest request,
            ICommandHandler<AddPatientCommand> handler,
            CancellationToken ct) =>
        {
            var command = new AddPatientCommand(
                ClientId: clientId,
                Name: request.Name,
                Species: request.Species,
                BirthDate: request.BirthDate);

            var result = await handler.Handle(command, ct);

            return result.ToNoContentHttpResult();
        });


        group.MapGet("/{id:guid}/patients", async (Guid id,
                IQueryHandler<GetClientPatientsQuery, IReadOnlyList<PatientDto>> handler,
                CancellationToken ct) =>
            {
                var result = await handler.Handle(new GetClientPatientsQuery(id), ct);

                return result.ToOkHttpResult(); 
            })
            .WithName("GetClientPatients");
        ;

        return app;
    }
}