using System.Runtime.Serialization;


[Serializable]
class PubliclyVisibleException : Exception
{
    /// <summary>
    /// Any message provided here will be returned to the client.
    /// </summary>
    public PubliclyVisibleException(string? message) : base(message)
    {
    }
}