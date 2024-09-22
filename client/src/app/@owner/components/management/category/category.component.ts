import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { ToastrService } from "ngx-toastr";
import { Subject, debounceTime, distinctUntilChanged } from "rxjs";
import { Pagination } from "src/app/core/models/interfaces/Common/Pagination";
import { ModalDeleteComponent } from "src/app/shared/components/modal-delete/modal-delete.component";
import { FormsModule } from "@angular/forms";
import { PaginationComponent } from "src/app/shared/components/pagination/pagination.component";
import { SpinnerComponent } from "src/app/shared/components/spinner/spinner.component";

import { MatButtonModule } from "@angular/material/button";
import { MatRadioModule } from "@angular/material/radio";
import { MatTableModule } from "@angular/material/table";
import { MatTooltipModule } from "@angular/material/tooltip";
import { NzButtonModule } from "ng-zorro-antd/button";
import { CategoryService } from "src/app/core/services/store/category.service";
import { Category } from "src/app/core/models/interfaces/Category";
import { FormAddComponent } from "./form-add/form-add.component";
import { FormEditComponent } from "./form-edit/form-edit.component";
import { LoaderService } from "src/app/core/services/loader.service";
import { Store } from "src/app/core/models/interfaces/Store";

const MatImport = [
	MatRadioModule,
	MatButtonModule,
	MatTableModule,
	MatDialogModule,
	MatTooltipModule,
	NzButtonModule,
];

@Component({
	selector: "app-category",
	standalone: true,
	imports: [
		CommonModule,
		MatImport,
		PaginationComponent,
		FormEditComponent,
		FormAddComponent,
		SpinnerComponent,
		FormsModule,
	],
	templateUrl: "./category.component.html",
	styleUrls: ["./category.component.scss"],
})
export class CategoryComponent {
	config = {
		displayedColumns: [
			{
				prop: "id",
				display: "STT",
			},
			{
				prop: "name",
				display: "Tên",
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
	listCategory: Category[] = [];
	store!: Store;
	private searchSubject = new Subject<string>();
	constructor(
		public dialog: MatDialog,
		private toastr: ToastrService,
		private categoryService: CategoryService,
		private loader: LoaderService
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
		this.store = JSON.parse(
			sessionStorage.getItem("storeInfo") ?? ""
		) as Store;
		this.loadListCategory();
	}

	loadListCategory(): void {
		this.categoryService
			.list(this.store.id, this.pagi, this.searchTerm)
			.subscribe({
				next: (res) => {
					this.listCategory = res.data.list;
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
		const dialogRef = this.dialog.open(FormAddComponent, {
			data: {
				idStore: this.store.id,
			},
		});
		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.loadListCategory();
			}
		});
	}

	openEditDialog(id: number): void {
		const dialogRef = this.dialog.open(FormEditComponent, {
			data: { id: id },
		});
		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.loadListCategory();
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
		this.categoryService.deleteById(id).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.toastr.success(res.message, "Xóa thành công", {
						timeOut: 3000,
					});
					this.loadListCategory();
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
		this.loadListCategory();
		console.log(searchTerm);
	}
}
