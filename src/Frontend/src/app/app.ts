import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { ShoppingBasketService } from './core/shopping-basket.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
})
export class App {
  protected readonly shoppingBasket = inject(ShoppingBasketService);

  constructor() {
    // The Shopping Basket count is in the header on every screen, so it is loaded once
    // when the client starts and refreshed by the service after every change.
    this.shoppingBasket.load();
  }
}
