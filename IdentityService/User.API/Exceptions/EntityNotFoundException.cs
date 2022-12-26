namespace User.API.Exceptions
{
    public class EntityNotFoundException: Exception
    {
        public EntityNotFoundException(): base() {}

        public EntityNotFoundException(string message): base(message) {}
    }
}
