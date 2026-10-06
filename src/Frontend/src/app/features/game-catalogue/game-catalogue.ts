import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { GameService } from '../../core/game.service';
import { ShoppingBasketService } from '../../core/shopping-basket.service';
import { describeHttpError } from '../../core/http-error';
import { Game } from '../../core/models/game';
import { MoneyPipe } from '../../shared/money.pipe';

@Component({
  selector: 'app-game-catalogue',
  imports: [RouterLink, MoneyPipe],
  templateUrl: './game-catalogue.html',
})
export class GameCatalogue {
  protected readonly games = inject(GameService);
  private readonly shoppingBasket = inject(ShoppingBasketService);

  /** The Game currently being added, so only its own button shows the spinner. */
  protected readonly addingGameId = signal<number | null>(null);
  protected readonly addedGameName = signal<string | null>(null);
  protected readonly addError = signal<string | null>(null);

  protected readonly loadingPlaceholders = [1, 2, 3, 4, 5, 6];

  constructor() {
    this.games.loadGames();
  }

  protected reload(): void {
    this.games.loadGames();
  }

  /** A Game's Image is an external URL; if it will not load, fall back to the tile. */
  protected onImageError(event: Event): void {
    (event.target as HTMLImageElement).style.visibility = 'hidden';
  }

  protected addToShoppingBasket(game: Game): void {
    this.addingGameId.set(game.id);
    this.addedGameName.set(null);
    this.addError.set(null);

    this.shoppingBasket.addGame(game.id).subscribe({
      next: () => {
        this.addingGameId.set(null);
        this.addedGameName.set(game.name);
      },
      error: (error: unknown) => {
        this.addingGameId.set(null);
        this.addError.set(describeHttpError(error));
      },
    });
  }
}
