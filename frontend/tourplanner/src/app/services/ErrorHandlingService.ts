import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ErrorHandlingService {

    public getErrorMessage(err: any): string {
    if (err.error?.message) return err.error.message;
    if (err.error?.errors) {
      return (Object.values(err.error.errors).flat() as string[]).join(', ');
    }
    if (err.error?.title) return err.error.title;
    return err.message;
  }
}