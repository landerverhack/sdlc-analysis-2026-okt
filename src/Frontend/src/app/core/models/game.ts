/**
 * A board game offered for sale — see docs/glossary.md.
 *
 * `price` and `currency` together are the Game's Money. The API splits Money into two
 * fields on the wire, so the client keeps them side by side and never formats one
 * without the other.
 */
export interface Game {
  id: number;
  name: string;
  price: number;
  currency: string;
  stockQuantity: number;
  publisherId: number;
  publisherName: string;
  imageUrl: string;
}
