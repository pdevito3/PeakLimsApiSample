namespace PeakLims.Domain.AccessionComments.Validators;

using PeakLims.Domain.AccessionComments.Dtos;
using FluentValidation;

public class AccessionCommentForManipulationDtoValidator<T> : AbstractValidator<T> where T : AccessionCommentForManipulationDto
{
    public AccessionCommentForManipulationDtoValidator()
    {
        // add fluent validation rules that should be shared between creation and update operations here
        //https://fluentvalidation.net/
    }

    // want to do some kind of db check to see if something is unique? try something like this with the `MustAsync` prop
    // source: https://github.com/jasontaylordev/CleanArchitecture/blob/413fb3a68a0467359967789e347507d7e84c48d4/src/Application/TodoLists/Commands/CreateTodoList/CreateTodoListCommandValidator.cs
    // public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    // {
    //     return await _context.TodoLists
    //         .AllAsync(l => l.Title != title, cancellationToken);
    // }
}