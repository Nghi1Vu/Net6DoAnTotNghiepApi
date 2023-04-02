using MediatR;

namespace Net6WebApiTemplate.Application.Clients.Queries.GetClientsQuery
{
    public class GetClientsQuery : IRequest<IList<ClientDto>>
    {
    }
}