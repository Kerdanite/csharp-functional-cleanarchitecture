using AspireCsharpFunctional.ApiService.Request;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using VetCalendar.Application.Abstractions;
using VetCalendar.Application.BookAppointment;

namespace AspireCsharpFunctional.ApiService.Endpoints;

public static class AppointmentEndpoints
{
    public static IEndpointRouteBuilder MapAppointmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/appointments");

        group.MapPost("/", async (
            BookAppointmentRequest request,
            ICommandHandler<BookAppointmentCommand> handler,
            CancellationToken ct) =>
        {
            var command = new BookAppointmentCommand(
                ClientId: request.ClientId,
                PatientId: request.PatientId,
                Date: request.Date,
                StartTime: request.StartTime,
                Reason: request.Reason);

            var result = await handler.Handle(command, ct);

            return result.ToNoContentHttpResult();
        });
                

        return app;
    }
}