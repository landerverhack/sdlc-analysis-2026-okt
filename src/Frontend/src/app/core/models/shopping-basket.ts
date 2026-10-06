/** One Game and a quantity within a Shopping Basket. */
export interface BasketLine {
  gameId: number;
  gameName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
  currency: string;
}

/** The Games a Customer intends to buy, before they buy them. */
export interface ShoppingBasket {
  id: number;
  customerId: number;
  subtotal: number;
  currency: string;
  lines: BasketLine[];
}

/**
 * The API has no Shopping Basket for a Customer until the first Game is added to it,
 * and answers 404 until then. The client treats that as an empty Shopping Basket.
 */
export function emptyShoppingBasket(customerId: number): ShoppingBasket {
  return {
    id: 0,
    customerId,
    subtotal: 0,
    currency: 'EUR',
    lines: [],
  };
}
