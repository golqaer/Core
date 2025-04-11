namespace Services.User.Provider;

public class UserProviderException : Exception
{
    public UserProviderException(string message, Exception innerException) : base(message, innerException) { }
    public UserProviderException(string message) : base(message) { }

    public class AccessDenied : UserProviderException
    {
        public const string
            NEED_USER = "Access denied, need`s auth",
            NEED_ADMIN = "Access denied, need`s admin"
            ;

        public AccessDenied(string message, Exception innerException) : base(message, innerException) { }
        public AccessDenied(string message) : base(message) { }
    }
}