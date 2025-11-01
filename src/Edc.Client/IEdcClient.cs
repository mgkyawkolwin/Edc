using Edc.Core.Common;
using Edc.Core.Messages;

namespace Edc.Client;

public interface IEdcClient
{
    // Task ConnectAsync(CancellationToken ct = default);
    // Task DisconnectAsync();
    Task<ResponseMessage> SendRequestAsync(RequestMessage requestMessage, CancellationToken cancellationToken = default, int timeOutMs = Constants.RESPONSE_TIMEOUT_MS);
}