import { Component, Inject, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";

import {
	MAT_DIALOG_DATA,
	MatDialog,
	MatDialogRef,
} from "@angular/material/dialog";
import { ToastrService } from "ngx-toastr";
import { NzFormModule } from "ng-zorro-antd/form";
import {
	ReactiveFormsModule,
	FormsModule,
	NonNullableFormBuilder,
	FormGroup,
	Validators,
} from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { Food } from "src/app/core/models/interfaces/Food";
import { OrderService } from "src/app/core/services/store/order.service";
import { OrderDetailService } from "src/app/core/services/store/order-detail.service";
import { Pagination } from "src/app/core/models/interfaces/Common/Pagination";
import { PaginationComponent } from "src/app/shared/components/pagination/pagination.component";
import { CategoryPipe } from "src/app/core/utils/category.pipe";
import { Order } from "src/app/core/models/interfaces/Order";
import { Invoice } from "src/app/core/models/interfaces/Invoice";
import { LoaderService } from "src/app/core/services/loader.service";
import { PricePipe } from "src/app/core/utils/price.pipe";
import { InvoiceService } from "src/app/core/services/store/invoice.service";
import { OrderDetailResponse } from "src/app/core/models/interfaces/Response/OrderDetailResponse";

const NzModule = [NzFormModule];

@Component({
	selector: "form-edit-food",
	standalone: true,
	imports: [
		CommonModule,
		MatButtonModule,
		ReactiveFormsModule,
		NzModule,
		FormsModule,
		PaginationComponent,
		CategoryPipe,
		PricePipe,
	],
	templateUrl: "./form-edit.component.html",
	styleUrls: ["./form-edit.component.scss"],
})
export class FormEditComponent implements OnInit {
	config = {
		displayedColumns: [
			{
				prop: "name",
				display: "Tên món ăn",
			},
			// {
			// 	prop: "images",
			// 	display: "Hình ảnh",
			// },
			{
				prop: "idCateogry",
				display: "Loại món ăn",
			},
			{
				prop: "quantity",
				display: "Số lượng",
			},
			{
				prop: "price",
				display: "Giá tiền",
			},
			{
				prop: "total",
				display: "Tổng tiền",
			},
		],
		hasAction: false,
	};

	pagi: Pagination = {
		totalPage: 0,
		totalRecords: 0,
		currentPage: 1,
		pageSize: 5,
		hasNextPage: false,
		hasPrevPage: false,
	};

	order!: Order;
	listFood!: Food[];
	listOrderDetail!: OrderDetailResponse[];
	validateForm!: FormGroup;

	createInvoice: boolean = false;

	constructor(
		public dialog: MatDialog,
		public dialogRef: MatDialogRef<FormEditComponent>,
		@Inject(MAT_DIALOG_DATA)
		public data: { idOrder: number; idStore: number },
		private orderService: OrderService,
		private orderDetailService: OrderDetailService,
		private invoiceService: InvoiceService,
		private loader: LoaderService,
		private toastr: ToastrService
	) {}
	ngOnInit(): void {
		this.loader.setLoading(true);
		this.orderService.getById(this.data.idOrder).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.order = res.data;
					this.order.idStore = this.data.idStore;
					this.loader.setLoading(false);
				}
			},
		});
		this.loadFood();
	}

	loadFood(): void {
		this.orderDetailService.list(this.data.idOrder, this.pagi).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.listOrderDetail = res.data.list;
					this.pagi = res.data.pagination;
					console.log(this.listOrderDetail);
				}
				console.log(res);
			},
		});
	}
	onChangePage(currentPage: any): void {
		this.pagi.currentPage = currentPage;
		this.loadFood();
	}

	onNoClick(): void {
		this.dialogRef.close(false);
	}

	onSubmit(): void {
		this.dialogRef.close(true);
	}

	// create invoice
	invoice: Invoice = {
		id: 0,
		createdAt: new Date(),
		finishedAt: new Date(),
		status: false,
		totalOrder: 0,
		charge: 0,
		total: 0,
		idOrder: 0,
		idPaymentType: 2,
	};
	openCreateInvoiceDialog(): void {
		// Opened form create
		if (this.createInvoice) {
			console.log("submit create invoice", this.invoice);
			this.invoiceService.create(this.invoice).subscribe({
				next: (res) => {
					if (res.isSuccess) {
						this.toastr.success(res.message, "Thông báo", {
							timeOut: 3000,
						});
					} else {
						this.toastr.error(res.message, "Thông báo", {
							timeOut: 3000,
						});
					}
				},
			});
		} else {
			this.invoice.totalOrder = this.order.total;
			this.invoice.idOrder = this.order.id;
			this.createInvoice = true;
			this.invoice.total = this.invoice.totalOrder + this.invoice.charge;
		}
	}
}
