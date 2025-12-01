using MediatR;
using Politico.Application.DTO.Contact;

namespace Politico.Application.Handlers.Admin.Contact.Queries
{
    public sealed record GetContactAdminQuery : IRequest<ContactAdminDto>;
}
