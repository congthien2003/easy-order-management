import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Pagination } from "src/app/core/models/interfaces/Common/Pagination";
import { Store } from "src/app/core/models/interfaces/Store";
import { ModalDeleteComponent } from "src/app/shared/components/modal-delete/modal-delete.component";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { ToastrService } from "ngx-toastr";
import { FormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatRadioModule } from "@angular/material/radio";
import { MatTableModule } from "@angular/material/table";
import { MatTooltipModule } from "@angular/material/tooltip";
import { NzButtonModule } from "ng-zorro-antd/button";
import { PaginationComponent } from "src/app/shared/components/pagination/pagination.component";
import { SpinnerComponent } from "src/app/shared/components/spinner/spinner.component";

import { QrcodeModule } from "qrcode-angular";
import { LoaderService } from "src/app/core/services/loader.service";
import { Table } from "src/app/core/models/interfaces/Table";
import { TableService } from "src/app/core/services/store/table.service";
import { ApiResponse } from "src/app/core/models/interfaces/Common/ApiResponse";
import { MatMenuModule } from "@angular/material/menu";
import { TemplateQrComponent } from "../../template-qr/template-qr.component";
const MatImport = [
	MatRadioModule,
	MatButtonModule,
	MatTableModule,
	MatDialogModule,
	MatTooltipModule,
	MatMenuModule,
];

@Component({
	selector: "app-table",
	standalone: true,
	imports: [
		CommonModule,
		FormsModule,
		SpinnerComponent,
		MatImport,
		PaginationComponent,
		QrcodeModule,
		TemplateQrComponent,
	],
	templateUrl: "./table.component.html",
	styleUrls: ["./table.component.scss"],
})
export class TableComponent implements OnInit {
	pagi: Pagination = {
		totalPage: 0,
		totalRecords: 0,
		currentPage: 1,
		pageSize: 8,
		hasNextPage: false,
		hasPrevPage: false,
	};
	selectedValueStatus: number = 0;
	filter: boolean = false;
	status: boolean = false;
	store!: Store;

	sizeQR: number = 120;
	listTable!: Table[];

	constructor(
		public dialog: MatDialog,
		private toastr: ToastrService,
		private loader: LoaderService,
		private tableService: TableService
	) {}

	ngOnInit(): void {
		this.store = JSON.parse(
			sessionStorage.getItem("storeInfo") ?? ""
		) as Store;

		this.loadListTable();
	}

	loadListTable(): void {
		this.tableService
			.list(this.store.id, this.pagi, this.filter, this.status)
			.subscribe({
				next: (res) => {
					if (res.isSuccess) {
						console.log(res.data.list);

						this.listTable = res.data.list;
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
		this.loadListTable();
	}

	onChangePage(currentPage: any): void {
		this.pagi.currentPage = currentPage;
		this.loadListTable();
	}

	downloadQR(qrcode: any, idTable: number): void {
		console.log(qrcode);

		const canvasEl = qrcode.canvas.nativeElement;
		if (canvasEl) {
			const imageUrl = canvasEl.toDataURL("image/png");
			const downloadLink = document.createElement("a");
			downloadLink.href = imageUrl;
			downloadLink.download = "qr-code-" + idTable + ".png"; // Set the file name
			downloadLink.click();
		}
	}

	addTable(): void {
		const newTable: Table = {
			id: 0,
			name: `Bàn`,
			status: true,
			idStore: this.store.id,
		};
		this.tableService.create(newTable).subscribe({
			next: (res) => {
				this.handleResponse(res);
			},
			error: (err) => {
				this.toastr.error(err.error.message, "Thất bại", {
					timeOut: 3000,
				});
			},
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
		this.tableService.deleteById(id).subscribe({
			next: (res) => {
				this.handleResponse(res);
			},
			error: (err) => {
				this.toastr.error(err.error.message, "Thất bại", {
					timeOut: 3000,
				});
			},
		});
	}

	handleResponse(res: ApiResponse) {
		if (res.isSuccess) {
			this.toastr.success(res.message, "Thành công", {
				timeOut: 3000,
			});

			this.loadListTable();
		} else {
			this.toastr.error(res.message, "Thất bại", {
				timeOut: 3000,
			});
		}
	}
}
