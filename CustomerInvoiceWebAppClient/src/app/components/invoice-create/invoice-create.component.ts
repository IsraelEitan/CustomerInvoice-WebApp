import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InvoiceService } from '../../services/invoice.service';

@Component({
  selector: 'app-invoice-create',
  templateUrl: './invoice-create.component.html',
  styleUrls: ['./invoice-create.component.css']
})
export class InvoiceCreateComponent {
  invoiceForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private invoiceService: InvoiceService
  ) {
    this.invoiceForm = this.formBuilder.group({
      date: ['', Validators.required],
      status: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(0.01)]],
      customerId: ['', [Validators.required, Validators.min(1)]]
    });
  }

  onSubmit() {
    if (this.invoiceForm.valid) {
      this.invoiceService.createInvoice(this.invoiceForm.value).subscribe(result => {
        console.log('Invoice Created', result);       
      });
    }
  }
}
