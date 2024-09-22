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
import { LoaderService } from "src/app/core/services/loader.service";
import { Food } from "src/app/core/models/interfaces/Food";
import { FoodService } from "src/app/core/services/store/food.service";
import { CategoryPipe } from "src/app/core/utils/category.pipe";
import { Category } from "src/app/core/models/interfaces/Category";
import { CategoryService } from "src/app/core/services/store/category.service";
import { Store } from "src/app/core/models/interfaces/Store";
import { FormAddComponent } from "./form-add/form-add.component";
import { FormEditComponent } from "./form-edit/form-edit.component";
import { FirebaseService } from "src/app/core/services/firebase.service";
import { ChangeDetectorRef } from "@angular/core";
import { PricePipe } from "src/app/core/utils/price.pipe";
const MatImport = [
	MatRadioModule,
	MatButtonModule,
	MatTableModule,
	MatDialogModule,
	MatTooltipModule,
	NzButtonModule,
];
@Component({
	selector: "app-food",
	standalone: true,
	imports: [
		CommonModule,
		MatImport,
		PaginationComponent,
		FormEditComponent,
		FormAddComponent,
		SpinnerComponent,
		FormsModule,
		CategoryPipe,
		PricePipe,
	],
	templateUrl: "./food.component.html",
	styleUrls: ["./food.component.scss"],
})
export class FoodComponent {
	config = {
		displayedColumns: [
			{
				prop: "id",
				display: "STT",
			},
			{
				prop: "images",
				display: "Hình ảnh",
			},
			{
				prop: "name",
				display: "Tên",
			},
			{
				prop: "price",
				display: "Giá",
			},
			// {
			// 	prop: "idCateogry",
			// 	display: "Loại món ăn",
			// },
		],
		hasAction: true,
	};
	pagi: Pagination = {
		totalPage: 0,
		totalRecords: 0,
		currentPage: 1,
		pageSize: 5,
		hasNextPage: false,
		hasPrevPage: false,
	};
	searchTerm: string = "";
	listFood!: Food[];
	listCategory!: Category[];

	store!: Store;

	private searchSubject = new Subject<string>();
	constructor(
		public dialog: MatDialog,
		private toastr: ToastrService,
		private foodService: FoodService,
		private categoryService: CategoryService,
		private firebaseSerivce: FirebaseService,
		private loader: LoaderService
	) {
		this.searchSubject
			.pipe(debounceTime(1500), distinctUntilChanged())
			.subscribe((searchTerm) => {
				this.search(searchTerm);
			});
	}
	ngOnInit(): void {
		this.store = JSON.parse(
			sessionStorage.getItem("storeInfo") ?? ""
		) as Store;

		this.loadListCategory();
		this.loadListFood();
	}

	loadListFood(): void {
		console.log("Load list food");

		this.foodService
			.list(this.store.id, this.pagi, this.searchTerm)
			.subscribe({
				next: (res) => {
					console.log(res);

					this.listFood = res.data.list;
					this.pagi = res.data.pagination;
				},
				error: (err) => {
					console.log(err);
				},
			});
	}

	getCategoryName(id: number): string {
		return this.listCategory.find((e) => e.id === id)?.name ?? "Unknown";
	}

	loadListCategory(): void {
		this.categoryService.getAllByIdStore(this.store.id).subscribe({
			next: (res) => {
				this.listCategory = res.data;
			},
			error: (err) => {
				console.log(err);
			},
		});
	}

	onChangePage(currentPage: any): void {
		this.pagi.currentPage = currentPage;
		this.loadListFood();
	}

	openAddDialog(): void {
		const dialogRef = this.dialog.open(FormAddComponent, {
			data: { idStore: this.store.id },
		});
		dialogRef.afterClosed().subscribe((result) => {
			if (result != null) {
				this.loadListFood();
			}
		});
	}

	openEditDialog(id: number): void {
		const dialogRef = this.dialog.open(FormEditComponent, {
			data: {
				id: id,
				listCategory: this.listCategory,
			},
		});
		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.loadListFood();
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

	async handleDelete(id: number): Promise<void> {
		try {
			const food = this.listFood.find((e) => {
				return e.id == id;
			});
			if (food) {
				await this.firebaseSerivce.deleteFileFromFirebase(
					food?.imageUrl
				);
				console.log("Xóa ảnh thành công !");
			}
		} catch (error) {
			console.log(error);
		}
		this.foodService.deleteById(id).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.toastr.success(res.message, "Xóa thành công", {
						timeOut: 3000,
					});
					this.loadListFood();
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
		this.loadListFood();
		console.log(searchTerm);
	}
}
