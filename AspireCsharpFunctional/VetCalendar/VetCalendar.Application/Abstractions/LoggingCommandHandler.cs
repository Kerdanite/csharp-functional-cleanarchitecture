using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Serilog.Context;


namespace VetCalendar.Application.Abstractions;

internal sealed class LoggingCommandHandler<TCommand>(
    ICommandHandler<TCommand> innerHandler,
    ILogger<ICommandHandler<TCommand>> logger)
    : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
    {
        string commandName = typeof(TCommand).Name;

        logger.LogInformation("Processing command {Command}", commandName);

        Result result = await innerHandler.Handle(command, cancellationToken);

        if (result.IsSuccess)
        {
            logger.LogInformation("Completed command {Command}", commandName);
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {
                logger.LogError("Completed command {Command} with error", commandName);
            }
        }

        return result;
    }
}
