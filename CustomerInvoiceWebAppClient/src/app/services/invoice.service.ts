import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Invoice } from '../models/invoice.model';
import { environment } from '../../enviroments/environment'
import { AppConfig } from '../app.config'; 

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private apiUrl = environment.apiUrl + '/invoices'; 

  constructor(private http: HttpClient) { }

  getAllInvoices(pageNumber: number = 1, pageSize: number = AppConfig.defaultPageSize): Observable<Invoice[]> {
    const queryParams = `?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.http.get<Invoice[]>(this.apiUrl + queryParams);
  }

  getInvoiceById(id: number): Observable<Invoice> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Invoice>(url).pipe(
      catchError(this.handleError)
    );
  }

  createInvoice(invoice: Invoice): Observable<Invoice> {
    return this.http.post<Invoice>(this.apiUrl, invoice).pipe(
      catchError(this.handleError)
    );
  }

  updateInvoice(id: number, invoice: Invoice): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.put(url, invoice).pipe(
      catchError(this.handleError)
    );
  }

  deleteInvoice(id: number): Observable<Invoice> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<Invoice>(url).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Something bad happened; please try again later.';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      console.error('An error occurred:', error.error.message);
      errorMessage = error.error.message;
    } else {
      // Server-side error
      console.error(`Backend returned code ${error.status}, body was:`, error.error);
      errorMessage = error.error?.message || errorMessage;
    }

    return throwError(errorMessage);
  }

}
