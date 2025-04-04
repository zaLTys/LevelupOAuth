public class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Authorization Header: " + request.Headers.Authorization);
        return await base.SendAsync(request, cancellationToken);
    }
}