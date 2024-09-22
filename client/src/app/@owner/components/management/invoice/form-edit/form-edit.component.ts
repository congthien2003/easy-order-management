import { Component, Inject, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { InvoiceService } from "src/app/core/services/store/invoice.service";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";
import { InvoiceResponse } from "src/app/core/models/interfaces/Response/InvoiceResponse";
import { OrderService } from "src/app/core/services/store/order.service";
import { Order } from "src/app/core/models/interfaces/Order";
import { OrderDetailService } from "src/app/core/services/store/order-detail.service";
import { Pagination } from "src/app/core/models/interfaces/Common/Pagination";
import { OrderDetailResponse } from "src/app/core/models/interfaces/Response/OrderDetailResponse";
import { PricePipe } from "src/app/core/utils/price.pipe";
import { PaginationComponent } from "src/app/shared/components/pagination/pagination.component";
import { LoaderService } from "src/app/core/services/loader.service";

@Component({
	selector: "app-form-edit",
	standalone: true,
	imports: [CommonModule, MatButtonModule, PricePipe, PaginationComponent],
	templateUrl: "./form-edit.component.html",
	styleUrls: ["./form-edit.component.scss"],
})
export class FormEditComponent implements OnInit {
	constructor(
		public dialogRef: MatDialogRef<FormEditComponent>,
		@Inject(MAT_DIALOG_DATA)
		public data: { invoice: InvoiceResponse },
		private loader: LoaderService,
		private orderDetail: OrderDetailService
	) {}

	config = {
		displayedColumns: [
			{
				prop: "name",
				display: "Tên món ăn",
			},
			{
				prop: "quantity",
				display: "Số lượng",
			},
			{
				prop: "total",
				display: "Tổng tiền",
			},
		],
		hasAction: false,
	};

	listOrderDetail: OrderDetailResponse[] = [];
	pagi: Pagination = {
		totalPage: 0,
		totalRecords: 0,
		currentPage: 1,
		pageSize: 9,
		hasNextPage: false,
		hasPrevPage: false,
	};
	ngOnInit(): void {
		this.loadFood();
	}

	loadFood(): void {
		this.loader.setLoading(true);
		this.orderDetail.list(this.data.invoice.order.id, this.pagi).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.loader.setLoading(false);

					this.listOrderDetail = res.data.list;
					this.pagi = res.data.pagination;
				}
			},
		});
	}

	onSubmit(): void {
		this.dialogRef.close(true);
	}

	onNoClick(): void {
		this.dialogRef.close(false);
	}

	onChangePage(currentPage: any): void {
		this.pagi.currentPage = currentPage;
		this.loadFood();
	}
}
