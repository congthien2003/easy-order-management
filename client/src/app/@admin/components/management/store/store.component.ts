import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Pagination } from "src/app/core/models/interfaces/Common/Pagination";
import { User } from "src/app/core/models/interfaces/User";

// Import Components
import { PaginationComponent } from "../../../../shared/components/pagination/pagination.component";
import { ModalDeleteComponent } from "src/app/shared/components/modal-delete/modal-delete.component";
import { FormAddComponent } from "../user/form-add/form-add.component";
import { FormEditComponent } from "../user/form-edit/form-edit.component";
import { MatDialog } from "@angular/material/dialog";
import { ToastrService } from "ngx-toastr";

@Component({
	selector: "app-store",
	standalone: true,
	templateUrl: "./store.component.html",
	styleUrls: ["./store.component.scss"],
	imports: [CommonModule, PaginationComponent],
})
export class StoreComponent implements OnInit {
	config = {
		displayedColumns: [
			{
				prop: "id",
				display: "STT",
			},
			{
				prop: "username",
				display: "Tên người dùng",
			},
			{
				prop: "email",
				display: "Email",
			},
			{
				prop: "phones",
				display: "Số điện thoại",
			},
			{
				prop: "role",
				display: "Vai trò",
			},
		],
		hasAction: true,
	};
	pagi: Pagination = {
		totalPage: 0,
		totalRecords: 0,
		currentPage: 1,
		pageSize: 15,
		hasNextPage: false,
		hasPrevPage: false,
	};
	listUser!: any[];

	constructor(public dialog: MatDialog, private toastr: ToastrService) {}

	ngOnInit(): void {
		throw new Error("Method not implemented.");
	}

	loadListStore(): void {}

	onChangePage(currentPage: any): void {
		console.log(currentPage);
		this.pagi.currentPage = currentPage;
	}

	openAddDialog(): void {
		const dialogRef = this.dialog.open(FormAddComponent);
		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				console.log("add form user");
				this.loadListStore();
			}
		});
	}

	openEditDialog(id: number): void {
		const dialogRef = this.dialog.open(FormEditComponent, {
			data: { id: id },
		});
		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.loadListStore();
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
		console.log("delete log ", id);
	}
}
