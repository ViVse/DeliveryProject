using GraphQL.Models;

namespace GraphQL.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic]
        public User OnUserAdded([EventMessage] User user) => user;
    }
}
