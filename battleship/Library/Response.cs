#nullable enable

namespace battleship.Library;

public class Response
{
    public int Status;
    public string? Error;

    public Response(int status, string? error = null)
    {
        Status = status;
        Error = error;
    }
}