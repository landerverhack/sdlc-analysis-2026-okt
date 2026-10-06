using GameStore.Domain.ValueObjects;
using System.Globalization;

namespace GameStore.Domain.Services;

internal interface ICultureToCurrencyService : IDomainService
{
  ValueTask<CurrencyName> CurrencyForCultureAsync(CultureInfo cultureInfo);
}