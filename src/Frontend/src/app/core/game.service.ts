import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, map } from 'rxjs';

import { API_BASE_URL } from './api.config';
import { LoadState } from './load-state';
import { describeHttpError } from './http-error';
import { Game } from './models/game';

/**
 * The wire shapes, as the endpoints actually assemble them
 * (src/Backend/src/GameStore.Presentation/Endpoints/GamesEndpoints.cs).
 *
 * `GET /api/games/{id}` calls the Game's name `title`, which the glossary forbids.
 * `GET /api/games` calls it `name`. Both are mapped onto Game here so the rest of the
 * client only ever sees glossary vocabulary.
 */
interface GameListItemResponse {
  id: number;
  name: string;
  price: number;
  currency: string;
  stockQuantity: number;
  publisherId: number;
  publisherName: string;
  imageUrl: string;
}

interface GameDetailResponse {
  id: number;
  title: string;
  price: number;
  currency: string;
  stockQuantity: number;
  publisherId: number;
  publisherName: string;
  imageUrl: string;
}

@Injectable({ providedIn: 'root' })
export class GameService {
  private readonly http = inject(HttpClient);

  private readonly gamesSignal = signal<readonly Game[]>([]);
  private readonly stateSignal = signal<LoadState>('idle');
  private readonly errorSignal = signal<string | null>(null);

  readonly games = this.gamesSignal.asReadonly();
  readonly state = this.stateSignal.asReadonly();
  readonly error = this.errorSignal.asReadonly();

  readonly isEmpty = computed(() => this.stateSignal() === 'loaded' && this.gamesSignal().length === 0);

  /** Loads the whole catalogue. There is no paging and no filtering in the API. */
  loadGames(): void {
    this.stateSignal.set('loading');
    this.errorSignal.set(null);

    this.http.get<GameListItemResponse[]>(`${API_BASE_URL}/games`).subscribe({
      next: (response) => {
        this.gamesSignal.set(response.map((game) => ({ ...game })));
        this.stateSignal.set('loaded');
      },
      error: (error: unknown) => {
        this.errorSignal.set(describeHttpError(error));
        this.stateSignal.set('error');
      },
    });
  }

  getGame(id: number): Observable<Game> {
    return this.http
      .get<GameDetailResponse>(`${API_BASE_URL}/games/${id}`)
      .pipe(map(({ title, ...rest }) => ({ ...rest, name: title })));
  }
}
