import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { GlobalErrorHandler } from './error-handler.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { PaginationComponent } from './components/pagination/pagination.component';
import { AppComponent } from './app.component';
import { InvoicesListComponent } from './components/invoices-list/invoices-list.component';
import { InvoiceCreateComponent } from './components/invoice-create/invoice-create.component';
import { InvoiceEditComponent } from './components/invoice-edit/invoice-edit.component';
import { ErrorComponent } from './components/error/error.component';

@NgModule({
  declarations: [
    AppComponent,
    ErrorComponent,
    InvoicesListComponent,
    InvoiceCreateComponent,
    InvoiceEditComponent,
    PaginationComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [{ provide: ErrorHandler, useClass: GlobalErrorHandler }],
  bootstrap: [AppComponent]
})
export class AppModule { }
