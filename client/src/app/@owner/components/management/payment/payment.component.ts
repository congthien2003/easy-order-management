import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { PaymentType } from "src/app/core/models/interfaces/PaymentType";
import { PaymentService } from "src/app/core/services/store/payment.service";
import { Store } from "src/app/core/models/interfaces/Store";
import { Pagination } from "src/app/core/models/interfaces/Common/Pagination";

@Component({
	selector: "app-payment",
	standalone: true,
	imports: [CommonModule, FormsModule, MatButtonModule],
	templateUrl: "./payment.component.html",
	styleUrls: ["./payment.component.scss"],
})
export class PaymentComponent {
	name: string = "";

	listPayment: PaymentType[] = [];
	pagi: Pagination = {
		totalPage: 0,
		totalRecords: 0,
		currentPage: 1,
		pageSize: 9,
		hasNextPage: false,
		hasPrevPage: false,
	};

	store!: Store;

	constructor(private paymentService: PaymentService) {
		this.store = JSON.parse(
			sessionStorage.getItem("storeInfo") ?? ""
		) as Store;
		this.paymentService.list(this.store.id, this.pagi).subscribe({
			next: (res) => {
				console.log(res);
				this.listPayment = res.data.list;
			},
		});
	}
}
