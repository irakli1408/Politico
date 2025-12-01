using MediatR;
using Politico.Application.DTO.Contact;

namespace Politico.Application.Handlers.Admin.Contact.Commands.Upsert
{
    public sealed record UpsertContactAdminCommand(ContactAdminDto Model) : IRequest<Unit>;
}
