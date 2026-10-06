import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'games' },
  {
    path: 'games',
    title: 'Game catalogue - GameStore',
    loadComponent: () => import('./features/game-catalogue/game-catalogue').then((m) => m.GameCatalogue),
  },
  {
    path: 'games/:id',
    title: 'Game - GameStore',
    loadComponent: () => import('./features/game-detail/game-detail').then((m) => m.GameDetail),
  },
  {
    path: 'shopping-basket',
    title: 'Shopping Basket - GameStore',
    loadComponent: () =>
      import('./features/shopping-basket/shopping-basket-page').then((m) => m.ShoppingBasketPage),
  },
  {
    path: 'checkout',
    title: 'Checkout - GameStore',
    loadComponent: () => import('./features/checkout/checkout').then((m) => m.Checkout),
  },
  { path: '**', redirectTo: 'games' },
];
