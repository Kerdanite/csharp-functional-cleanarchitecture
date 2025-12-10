using CSharpFunctionalExtensions;
using System.Windows.Input;

namespace VetCalendar.Application.Abstractions;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}
