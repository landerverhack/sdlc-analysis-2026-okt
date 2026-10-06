/**
 * Where the GameStore API lives, and who is shopping.
 *
 * There is no authentication yet. The API takes the Customer in the route
 * (`/api/baskets/{customerId}`), so the client shops as a single hardcoded Customer,
 * which the development seed data creates with id 1.
 */
export const API_BASE_URL = 'http://localhost:5038/api';

export const CUSTOMER_ID = 1;
