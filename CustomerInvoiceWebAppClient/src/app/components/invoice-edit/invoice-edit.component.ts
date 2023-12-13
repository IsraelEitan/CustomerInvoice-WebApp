import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { InvoiceService } from '../../services/invoice.service';

@Component({
  selector: 'app-invoice-edit',
  templateUrl: './invoice-edit.component.html',
  styleUrls: ['./invoice-edit.component.css']
})
export class InvoiceEditComponent implements OnInit {
  invoiceForm: FormGroup;
  invoiceId!: number;

  constructor(
    private formBuilder: FormBuilder,
    private invoiceService: InvoiceService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.invoiceForm = this.formBuilder.group({
      date: ['', Validators.required],
      status: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(0.01)]],
      customerId: ['', [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.invoiceId = +idParam;
      this.invoiceService.getInvoiceById(this.invoiceId).subscribe(invoice => {
        this.invoiceForm.patchValue(invoice);
      });
    } else {
      //Redirect
      console.error('Invoice ID is not provided');
      this.router.navigate(['/invoices']);
    }
  }

  onSubmit() {
    if (this.invoiceForm.valid) {
      this.invoiceService.updateInvoice(this.invoiceId, this.invoiceForm.value).subscribe(result => {
        console.log('Invoice Updated', result);
        //Navigate to the list of invoices
        this.router.navigate(['/invoices']); 
      });
    }
  }
}
