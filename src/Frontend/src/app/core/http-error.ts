import { HttpErrorResponse } from '@angular/common/http';

/**
 * Turns whatever the API (or the browser) produced into a sentence a Customer can read.
 * The API reports a broken business rule as `400 { "error": "..." }`.
 */
export function describeHttpError(error: unknown): string {
  if (error instanceof HttpErrorResponse) {
    if (error.status === 0) {
      return 'The GameStore API could not be reached. Check that it is running on http://localhost:5038.';
    }

    const body = error.error as { error?: string } | null;
    if (body?.error) {
      return body.error;
    }

    return `The GameStore API answered ${error.status} ${error.statusText}.`;
  }

  return 'Something went wrong.';
}
