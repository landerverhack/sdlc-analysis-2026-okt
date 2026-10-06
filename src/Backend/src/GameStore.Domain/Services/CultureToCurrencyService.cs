using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using System.Globalization;

namespace GameStore.Domain.Services;

internal sealed class CultureToCurrencyService : ICultureToCurrencyService
{
  public ValueTask<CurrencyName> CurrencyForCultureAsync(CultureInfo cultureInfo)
  {
    RegionInfo? region = new RegionInfo(cultureInfo.Name);
    string currencyNameSymbol = region.ISOCurrencySymbol;
    CurrencyName name = Currency.Parse(currencyNameSymbol);
    return ValueTask.FromResult(name);
  }
}
