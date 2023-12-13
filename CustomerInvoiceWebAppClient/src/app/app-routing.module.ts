import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoicesListComponent } from './components/invoices-list/invoices-list.component';
import { InvoiceCreateComponent } from './components/invoice-create/invoice-create.component';
import { InvoiceEditComponent } from './components/invoice-edit/invoice-edit.component';
import { ErrorComponent } from './components/error/error.component';

const routes: Routes = [
  { path: '', redirectTo: '/invoices', pathMatch: 'full' },
  { path: 'invoices', component: InvoicesListComponent },
  { path: 'invoices/create', component: InvoiceCreateComponent },
  { path: 'invoices/edit/:id', component: InvoiceEditComponent },
  { path: 'error', component: ErrorComponent },
  { path: '**', redirectTo: '/error' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
