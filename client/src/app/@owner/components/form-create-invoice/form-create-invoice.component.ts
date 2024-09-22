import { Component, Inject } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Order } from "src/app/core/models/interfaces/Order";
import { Invoice } from "src/app/core/models/interfaces/Invoice";

@Component({
	selector: "owner-form-create-invoice",
	standalone: true,
	imports: [CommonModule],
	templateUrl: "./form-create-invoice.component.html",
	styleUrls: ["./form-create-invoice.component.scss"],
})
export class FormCreateInvoiceComponent {
	invoice: Invoice = {
		id: 0,
		createdAt: new Date(),
		finishedAt: new Date(),
		status: false,
		totalOrder: 0,
		charge: 0,
		total: 0,
		idOrder: 0,
		idPaymentType: 0,
	};

	constructor(
		public dialogRef: MatDialogRef<FormCreateInvoiceComponent>,
		@Inject(MAT_DIALOG_DATA)
		public data: {
			order: Order;
		}
	) {
		this.invoice.totalOrder = this.data.order.total;
		this.invoice.idOrder = this.data.order.id;
		console.log(this.data.order);
	}
}
