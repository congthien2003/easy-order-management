import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { InvoiceService } from "src/app/core/services/store/invoice.service";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { ToastrService } from "ngx-toastr";
import { Pagination } from "src/app/core/models/interfaces/Common/Pagination";
import { Store } from "src/app/core/models/interfaces/Store";
import { Invoice } from "src/app/core/models/interfaces/Invoice";
import { FormEditComponent } from "./form-edit/form-edit.component";
import { ModalDeleteComponent } from "src/app/shared/components/modal-delete/modal-delete.component";
import { FormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatRadioModule } from "@angular/material/radio";
import { MatTableModule } from "@angular/material/table";
import { MatTooltipModule } from "@angular/material/tooltip";
import { NzButtonModule } from "ng-zorro-antd/button";
import { PaginationComponent } from "src/app/shared/components/pagination/pagination.component";
import { SpinnerComponent } from "src/app/shared/components/spinner/spinner.component";
import { MatMenuModule } from "@angular/material/menu";
import { PricePipe } from "src/app/core/utils/price.pipe";
import { TableService } from "src/app/core/services/store/table.service";
import { InvoiceResponse } from "src/app/core/models/interfaces/Response/InvoiceResponse";
import { Table } from "src/app/core/models/interfaces/Table";
import { map, Observable } from "rxjs";
import { LoaderService } from "src/app/core/services/loader.service";

const MatImport = [
	MatRadioModule,
	MatButtonModule,
	MatTableModule,
	MatDialogModule,
	MatTooltipModule,
	NzButtonModule,
	MatMenuModule,
];

@Component({
	selector: "app-invoice",
	standalone: true,
	imports: [
		CommonModule,
		MatImport,
		PaginationComponent,
		FormEditComponent,
		SpinnerComponent,
		FormsModule,
		PricePipe,
	],
	templateUrl: "./invoice.component.html",
	styleUrls: ["./invoice.component.scss"],
})
export class InvoiceComponent implements OnInit {
	config = {
		displayedColumns: [
			{
				prop: "id",
				display: "STT",
			},
			{
				prop: "tableName",
				display: "Bàn",
			},
			{
				prop: "status",
				display: "Trạng thái",
			},
		],
		hasAction: true,
	};
	pagi: Pagination = {
		totalPage: 0,
		totalRecords: 0,
		currentPage: 1,
		pageSize: 20,
		hasNextPage: false,
		hasPrevPage: false,
	};
	selectedValueStatus: number = 0;
	filter: boolean = false;
	status: boolean = false;
	sortColumn: string = "";
	store!: Store;
	listInvoice: InvoiceResponse[] = [];
	listTable: Table[] = [];

	constructor(
		public dialog: MatDialog,
		private toastr: ToastrService,
		private invoiceService: InvoiceService,
		private tableService: TableService,
		private loader: LoaderService
	) {}
	ngOnInit(): void {
		this.store = JSON.parse(
			sessionStorage.getItem("storeInfo") ?? ""
		) as Store;

		this.loadListInvoice();
	}

	loadListInvoice(): void {
		this.invoiceService
			.list(
				this.store.id,
				this.pagi,
				this.sortColumn,
				this.filter,
				this.status
			)
			.subscribe({
				next: (res) => {
					if (res.isSuccess) {
						console.log(res.data);

						this.listInvoice = res.data.list;

						this.pagi = res.data.pagination;
					}
				},
			});
	}

	changeFilter(value: number) {
		this.selectedValueStatus = value;
		switch (this.selectedValueStatus) {
			case 1: {
				this.filter = true;
				this.status = true;
				break;
			}
			case 2: {
				this.filter = true;
				this.status = false;
				break;
			}
			default: {
				this.filter = false;
				this.status = false;
			}
		}
		this.loadListInvoice();
	}

	onChangePage(currentPage: any): void {
		this.pagi.currentPage = currentPage;
		this.loadListInvoice();
	}

	openEditDialog(id: number): void {
		const dialogRef = this.dialog.open(FormEditComponent, {
			data: {
				invoice: this.listInvoice.find((e) => e.id === id),
			},
		});
		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.invoiceService.aceept(id).subscribe({
					next: (res) => {
						if (res.isSuccess) {
							this.toastr.success(res.message, "Thành công", {
								timeOut: 3000,
							});
							this.loadListInvoice();
						}
					},
				});
			}
		});
	}

	openDeleteDialog(id: number): void {
		const dialogRef = this.dialog.open(ModalDeleteComponent, {
			data: { id: id },
		});
		dialogRef.afterClosed().subscribe((result) => {
			if (result === true) {
				this.handleDelete(id);
			}
		});
	}

	handleDelete(id: number): void {
		this.invoiceService.deleteById(id).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.toastr.success(res.message, "Thông báo", {
						timeOut: 3000,
					});
					this.loadListInvoice();
				} else {
					this.toastr.error(res.message, "Thông báo", {
						timeOut: 3000,
					});
				}
			},
		});
	}
}
