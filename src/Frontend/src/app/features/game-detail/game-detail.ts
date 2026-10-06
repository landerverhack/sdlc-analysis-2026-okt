import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { GameService } from '../../core/game.service';
import { ShoppingBasketService } from '../../core/shopping-basket.service';
import { describeHttpError } from '../../core/http-error';
import { LoadState } from '../../core/load-state';
import { Game } from '../../core/models/game';
import { MoneyPipe } from '../../shared/money.pipe';

@Component({
  selector: 'app-game-detail',
  imports: [RouterLink, MoneyPipe],
  templateUrl: './game-detail.html',
})
export class GameDetail {
  private readonly games = inject(GameService);
  private readonly shoppingBasket = inject(ShoppingBasketService);

  /** Bound from the `games/:id` route. */
  readonly id = input.required<string>();

  protected readonly game = signal<Game | null>(null);
  protected readonly state = signal<LoadState>('idle');
  protected readonly error = signal<string | null>(null);
  protected readonly notFound = signal(false);

  protected readonly quantity = signal(1);
  protected readonly adding = signal(false);
  protected readonly added = signal(false);
  protected readonly addError = signal<string | null>(null);

  /** A Customer cannot order more copies than the Stock Quantity. */
  protected readonly maxQuantity = computed(() => Math.max(this.game()?.stockQuantity ?? 1, 1));

  constructor() {
    effect(() => this.load(Number(this.id())));
  }

  protected load(gameId: number): void {
    this.state.set('loading');
    this.error.set(null);
    this.notFound.set(false);
    this.added.set(false);
    this.addError.set(null);
    this.quantity.set(1);

    this.games.getGame(gameId).subscribe({
      next: (game) => {
        this.game.set(game);
        this.state.set('loaded');
      },
      error: (error: unknown) => {
        if (error instanceof HttpErrorResponse && error.status === 404) {
          this.notFound.set(true);
          this.state.set('loaded');
          return;
        }

        this.error.set(describeHttpError(error));
        this.state.set('error');
      },
    });
  }

  protected reload(): void {
    this.load(Number(this.id()));
  }

  /** A Game's Image is an external URL; if it will not load, fall back to the tile. */
  protected onImageError(event: Event): void {
    (event.target as HTMLImageElement).style.visibility = 'hidden';
  }

  protected changeQuantity(by: number): void {
    const next = this.quantity() + by;
    this.quantity.set(Math.min(Math.max(next, 1), this.maxQuantity()));
  }

  protected addToShoppingBasket(game: Game): void {
    this.adding.set(true);
    this.added.set(false);
    this.addError.set(null);

    this.shoppingBasket.addGame(game.id, this.quantity()).subscribe({
      next: () => {
        this.adding.set(false);
        this.added.set(true);
      },
      error: (error: unknown) => {
        this.adding.set(false);
        this.addError.set(describeHttpError(error));
      },
    });
  }
}
