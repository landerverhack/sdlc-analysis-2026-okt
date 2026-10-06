import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, map, tap } from 'rxjs';

import { API_BASE_URL, CUSTOMER_ID } from './api.config';
import { LoadState } from './load-state';
import { describeHttpError } from './http-error';
import { BasketLine, ShoppingBasket, emptyShoppingBasket } from './models/shopping-basket';

/**
 * The wire shape of `GET /api/baskets/{customerId}`
 * (src/Backend/src/GameStore.Presentation/Endpoints/BasketsEndpoints.cs).
 * A Basket Line calls the Game's name `title` there, which the glossary forbids, so it
 * is mapped to `gameName` on the way in.
 */
interface ShoppingBasketResponse {
  id: number;
  customerId: number;
  subtotal: number;
  currency: string;
  lines: {
    gameId: number;
    title: string;
    quantity: number;
    unitPrice: number;
    lineTotal: number;
    currency: string;
  }[];
}

/**
 * Holds the one Shopping Basket this client knows about, for the hardcoded Customer.
 * State is a handful of signals: there is one basket, and every screen reads the same
 * one.
 */
@Injectable({ providedIn: 'root' })
export class ShoppingBasketService {
  private readonly http = inject(HttpClient);

  private readonly basketSignal = signal<ShoppingBasket>(emptyShoppingBasket(CUSTOMER_ID));
  private readonly stateSignal = signal<LoadState>('idle');
  private readonly errorSignal = signal<string | null>(null);

  readonly basket = this.basketSignal.asReadonly();
  readonly state = this.stateSignal.asReadonly();
  readonly error = this.errorSignal.asReadonly();

  readonly lines = computed<readonly BasketLine[]>(() => this.basketSignal().lines);
  readonly subtotal = computed(() => this.basketSignal().subtotal);
  readonly currency = computed(() => this.basketSignal().currency);
  readonly isEmpty = computed(() => this.basketSignal().lines.length === 0);

  /** How many copies of Games are in the Shopping Basket, across all Basket Lines. */
  readonly gameCount = computed(() =>
    this.basketSignal().lines.reduce((count, line) => count + line.quantity, 0),
  );

  load(): void {
    this.stateSignal.set('loading');
    this.errorSignal.set(null);

    this.http.get<ShoppingBasketResponse>(`${API_BASE_URL}/baskets/${CUSTOMER_ID}`).subscribe({
      next: (response) => {
        this.basketSignal.set({
          id: response.id,
          customerId: response.customerId,
          subtotal: response.subtotal,
          currency: response.currency,
          lines: response.lines.map((line) => ({
            gameId: line.gameId,
            gameName: line.title,
            quantity: line.quantity,
            unitPrice: line.unitPrice,
            lineTotal: line.lineTotal,
            currency: line.currency,
          })),
        });
        this.stateSignal.set('loaded');
      },
      error: (error: unknown) => {
        // No Shopping Basket exists until the first Game is added, and the API answers
        // 404. That is an empty Shopping Basket to a Customer, not a failure.
        if (error instanceof HttpErrorResponse && error.status === 404) {
          this.basketSignal.set(emptyShoppingBasket(CUSTOMER_ID));
          this.stateSignal.set('loaded');
          return;
        }

        this.errorSignal.set(describeHttpError(error));
        this.stateSignal.set('error');
      },
    });
  }

  /**
   * Adds a Game to the Shopping Basket. Adding a Game that is already there increases
   * the quantity of its Basket Line — the aggregate decides that, not the client.
   *
   * The API answers with the Shopping Basket's identity only, so the Basket is reloaded
   * to get the new Basket Lines and Subtotal.
   */
  addGame(gameId: number, quantity = 1): Observable<void> {
    return this.http
      .post(`${API_BASE_URL}/baskets/${CUSTOMER_ID}/items`, { gameId, quantity })
      .pipe(
        tap(() => this.load()),
        map(() => undefined),
      );
  }

  /** Turns the Shopping Basket into an Order. The API answers 202 with no body. */
  checkout(): Observable<void> {
    return this.http.post(`${API_BASE_URL}/baskets/${CUSTOMER_ID}/checkout`, {}).pipe(
      tap(() => this.load()),
      map(() => undefined),
    );
  }
}
