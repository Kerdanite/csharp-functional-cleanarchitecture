using CSharpFunctionalExtensions;

namespace VetCalendar.Application.Abstractions;

public class UnitOfWorkCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _inner;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<TCommand> inner,
        IUnitOfWork unitOfWork)
    {
        _inner = inner;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(TCommand command, CancellationToken ct = default)
    {
        await _unitOfWork.BeginAsync(ct);

        try
        {
            var result = await _inner.Handle(command, ct);

            if (result.IsFailure)
            {
                await _unitOfWork.RollbackAsync(ct);
                return result;
            }

            await _unitOfWork.CommitAsync(ct);
            return result;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}