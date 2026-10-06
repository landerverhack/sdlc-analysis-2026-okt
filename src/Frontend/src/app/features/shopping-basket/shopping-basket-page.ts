import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { ShoppingBasketService } from '../../core/shopping-basket.service';
import { describeHttpError } from '../../core/http-error';
import { BasketLine } from '../../core/models/shopping-basket';
import { MoneyPipe } from '../../shared/money.pipe';

@Component({
  selector: 'app-shopping-basket-page',
  imports: [RouterLink, MoneyPipe],
  templateUrl: './shopping-basket-page.html',
})
export class ShoppingBasketPage {
  protected readonly shoppingBasket = inject(ShoppingBasketService);

  protected readonly changingGameId = signal<number | null>(null);
  protected readonly changeError = signal<string | null>(null);

  protected readonly loadingPlaceholders = [1, 2, 3];

  constructor() {
    this.shoppingBasket.load();
  }

  protected reload(): void {
    this.shoppingBasket.load();
  }

  /**
   * The API exposes one way to change a Shopping Basket: add a Game. Adding a Game that
   * is already on a Basket Line increases that line's quantity. There is no endpoint for
   * decreasing a quantity or removing a Basket Line, so this screen cannot offer either.
   */
  protected addOneMore(line: BasketLine): void {
    this.changingGameId.set(line.gameId);
    this.changeError.set(null);

    this.shoppingBasket.addGame(line.gameId).subscribe({
      next: () => this.changingGameId.set(null),
      error: (error: unknown) => {
        this.changingGameId.set(null);
        this.changeError.set(describeHttpError(error));
      },
    });
  }
}
