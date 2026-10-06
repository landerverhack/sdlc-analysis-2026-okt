import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { ShoppingBasketService } from '../../core/shopping-basket.service';
import { describeHttpError } from '../../core/http-error';
import { MoneyPipe } from '../../shared/money.pipe';

@Component({
  selector: 'app-checkout',
  imports: [RouterLink, MoneyPipe],
  templateUrl: './checkout.html',
})
export class Checkout {
  protected readonly shoppingBasket = inject(ShoppingBasketService);

  protected readonly placing = signal(false);
  protected readonly placed = signal(false);
  protected readonly placeError = signal<string | null>(null);

  protected readonly loadingPlaceholders = [1, 2, 3];

  constructor() {
    this.shoppingBasket.load();
  }

  protected reload(): void {
    this.shoppingBasket.load();
  }

  /**
   * Checkout turns the Shopping Basket into an Order and clears the Basket.
   *
   * The API answers 202 Accepted with no body, so the client has no Order Number to
   * show and cannot report the Discount or the Final Total that were fixed at this
   * moment. All it can honestly say is that the Order was placed.
   */
  protected placeOrder(): void {
    this.placing.set(true);
    this.placeError.set(null);

    this.shoppingBasket.checkout().subscribe({
      next: () => {
        this.placing.set(false);
        this.placed.set(true);
      },
      error: (error: unknown) => {
        this.placing.set(false);
        this.placeError.set(describeHttpError(error));
      },
    });
  }
}
