import { ErrorHandler, Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private router: Router) { }

  handleError(error: any): void {
    console.error('Global error handler:', error);
    this.router.navigate(['/error']);
  }
}
