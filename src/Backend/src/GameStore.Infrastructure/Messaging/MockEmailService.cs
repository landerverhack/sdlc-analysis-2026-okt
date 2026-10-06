using GameStore.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace GameStore.Infrastructure.Messaging;

internal sealed class MockEmailService(ILogger<MockEmailService> logger) : IEmailService
{
  public Task SendAsync(
    string to,
    string subject,
    string body,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "Mock email sent to {Recipient}. Subject: {Subject}. Body: {Body}",
      to,
      subject,
      body);

    return Task.CompletedTask;
  }
}
