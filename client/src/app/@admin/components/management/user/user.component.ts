import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";

import { ToastrService } from "ngx-toastr";
import { UserService } from "src/app/core/services/user/user.service";
import { Pagination } from "src/app/core/models/interfaces/Common/Pagination";
import { User } from "src/app/core/models/interfaces/User";
import { PaginationComponent } from "src/app/shared/components/pagination/pagination.component";

import { ModalDeleteComponent } from "src/app/shared/components/modal-delete/modal-delete.component";
import { FormEditComponent } from "./form-edit/form-edit.component";
import { SpinnerComponent } from "src/app/shared/components/spinner/spinner.component";
import { RolePipe } from "src/app/core/utils/role.pipe";
import { FormsModule } from "@angular/forms";

// Import Mat
import { MatRadioModule } from "@angular/material/radio";
import { MatButtonModule } from "@angular/material/button";
import { MatDialog } from "@angular/material/dialog";
import { MatTableModule } from "@angular/material/table";
import { MatDialogModule } from "@angular/material/dialog";
import { MatTooltipModule } from "@angular/material/tooltip";
import { FormAddComponent } from "./form-add/form-add.component";
import { NzButtonModule } from "ng-zorro-antd/button";

import { debounceTime, distinctUntilChanged, Subject, timeout } from "rxjs";
import { LoaderService } from "src/app/core/services/loader.service";
const MatImport = [
	MatRadioModule,
	MatButtonModule,
	MatTableModule,
	MatDialogModule,
	MatTooltipModule,
	NzButtonModule,
];

@Component({
	selector: "app-user",
	standalone: true,
	templateUrl: "./user.component.html",
	styleUrls: ["./user.component.scss"],
	imports: [
		CommonModule,
		MatImport,
		PaginationComponent,
		FormEditComponent,
		FormAddComponent,
		SpinnerComponent,
		RolePipe,
		FormsModule,
	],
})
export class UserComponent implements OnInit {
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
	searchTerm: string = "";
	listUser!: User[];
	private searchSubject = new Subject<string>();
	constructor(
		public dialog: MatDialog,
		private toastr: ToastrService,
		private userService: UserService
	) {
		this.searchSubject
			.pipe(
				debounceTime(1500), // Đợi 1.5 giây sau khi người dùng ngừng nhập
				distinctUntilChanged() // Chỉ phát khi giá trị khác với giá trị trước đó
			)
			.subscribe((searchTerm) => {
				this.search(searchTerm);
			});
	}
	ngOnInit(): void {
		this.loadListUser();
	}

	loadListUser(): void {
		this.userService.list(this.pagi, this.searchTerm).subscribe({
			next: (res) => {
				this.listUser = res.data.list;
				this.pagi = res.data.pagination;
			},
			error: (err) => {
				console.log(err);
			},
		});
	}

	onChangePage(currentPage: any): void {
		console.log(currentPage);
		this.pagi.currentPage = currentPage;
	}

	openAddDialog(): void {
		const dialogRef = this.dialog.open(FormAddComponent);
		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				console.log("add form user");
				this.loadListUser();
			}
		});
	}

	openEditDialog(id: number): void {
		const dialogRef = this.dialog.open(FormEditComponent, {
			data: { id: id },
		});
		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.loadListUser();
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
		this.userService.deleteById(id).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.toastr.success(res.message, "Xóa thành công", {
						timeOut: 3000,
					});
					this.loadListUser();
				} else {
					this.toastr.error(res.message, "Xóa không thành công", {
						timeOut: 3000,
					});
				}
			},
			error: () => {
				this.toastr.error("", "Có lỗi xảy ra", {
					timeOut: 3000,
				});
			},
		});
	}

	onSearchTerm(): void {
		this.searchSubject.next(this.searchTerm);
	}

	search(searchTerm: string): void {
		this.loadListUser();
		console.log(searchTerm);
	}
}
