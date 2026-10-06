import { Pipe, PipeTransform } from '@angular/core';

/**
 * Formats Money. The API splits Money into an amount and a currency, so both are
 * required here — an amount without a currency is not a price.
 */
@Pipe({ name: 'money' })
export class MoneyPipe implements PipeTransform {
  transform(amount: number | null | undefined, currency: string | null | undefined): string {
    if (amount === null || amount === undefined) {
      return '';
    }

    return new Intl.NumberFormat('nl-BE', {
      style: 'currency',
      currency: currency ?? 'EUR',
    }).format(amount);
  }
}
