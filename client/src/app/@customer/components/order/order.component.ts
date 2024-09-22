import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ActivatedRoute } from "@angular/router";
import { StoreService } from "src/app/core/services/store/store.service";
import { OrderService } from "src/app/core/services/store/order.service";
import { FoodService } from "src/app/core/services/store/food.service";
import { CategoryService } from "src/app/core/services/store/category.service";
import { OrderDetailService } from "src/app/core/services/store/order-detail.service";
import { Store } from "src/app/core/models/interfaces/Store";
import { Category } from "src/app/core/models/interfaces/Category";
import { Food } from "src/app/core/models/interfaces/Food";
import { FormsModule } from "@angular/forms";
import { Pagination } from "src/app/core/models/interfaces/Common/Pagination";
import { CategoryPipe } from "src/app/core/utils/category.pipe";
import { PricePipe } from "src/app/core/utils/price.pipe";
// Import Mat
import { MatChipsModule } from "@angular/material/chips";
import { MatButtonModule } from "@angular/material/button";
import { MatBadgeModule } from "@angular/material/badge";
import { ToastrService } from "ngx-toastr";
import { OrderDetail } from "src/app/core/models/interfaces/OrderDetail";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { CartComponent } from "../cart/cart.component";
import { Order } from "src/app/core/models/interfaces/Order";
import { PaginationComponent } from "src/app/shared/components/pagination/pagination.component";
const MatImport = [
	MatButtonModule,
	MatChipsModule,
	MatBadgeModule,
	MatDialogModule,
];

@Component({
	selector: "app-order",
	standalone: true,
	imports: [
		CommonModule,
		MatImport,
		FormsModule,
		CategoryPipe,
		PricePipe,
		PaginationComponent,
	],
	templateUrl: "./order.component.html",
	styleUrls: ["./order.component.scss"],
})
export class OrderComponent implements OnInit {
	idStore: number = 0;
	idTable: number = 0;

	store!: Store;
	listCategory: Category[] = [];
	listFood: Food[] = [];

	pagiFood: Pagination = {
		totalPage: 0,
		totalRecords: 0,
		currentPage: 1,
		pageSize: 10,
		hasNextPage: false,
		hasPrevPage: false,
	};

	search: string = "";
	selectedCategory: number = 0;
	listOrderDetail: OrderDetail[] = [];

	order: Order = {
		id: 0,
		total: 0,
		status: false,
		createdAt: new Date(),
		idTable: 0,
		idStore: 0,
	};

	submitOrderStatus: boolean = false;

	constructor(
		private activeRoute: ActivatedRoute,
		private storeService: StoreService,
		private foodService: FoodService,
		private categoryService: CategoryService,
		private orderService: OrderService,
		private orderDetailService: OrderDetailService,
		private toastr: ToastrService,
		private dialog: MatDialog
	) {}

	ngOnInit(): void {
		this.idStore = this.activeRoute.snapshot.params["idStore"];
		this.idTable = this.activeRoute.snapshot.params["idTable"];

		this.storeService.getById(this.idStore).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.store = res.data;

					this.order.idStore = this.idStore;
					this.order.idTable = this.idTable;
				}
			},
		});

		this.categoryService.getAllByIdStore(this.idStore).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.listCategory = res.data;
				}
			},
		});
		this.loadListFood();
	}

	loadListFood(): void {
		this.foodService.list(this.idStore, this.pagiFood).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.listFood = res.data.list;
					this.pagiFood = res.data.pagination;
				}
			},
		});
	}

	onChangePage(currentPage: any): void {
		this.pagiFood.currentPage = currentPage;
		this.loadListFood();
	}

	onChangeCategory(id: number): void {
		this.selectedCategory = id;
		if (this.selectedCategory === 0) {
			this.loadListFood();
		} else {
			this.foodService
				.getByCategory(this.selectedCategory, this.pagiFood)
				.subscribe({
					next: (res) => {
						if (res.isSuccess) {
							this.listFood = res.data.list;
							this.pagiFood = res.data.pagination;
						} else {
							console.log(res.message);
						}
					},
				});
		}
	}

	addToCart(id: number): void {
		let hasInList = false;
		for (let i = 0; i < this.listOrderDetail.length; i++) {
			if (this.listOrderDetail[i].idFood === id) {
				this.listOrderDetail[i].quantity++;
				hasInList = true;
				break;
			}
		}
		if (hasInList === false) {
			this.listOrderDetail.push({ idFood: id, quantity: 1, idOrder: 0 });
		}
		this.toastr.success("Thêm thành công", "Thành công", { timeOut: 2500 });
	}

	openCart(): void {
		const dialogRef = this.dialog.open(CartComponent, {
			data: {
				listOrderDetail: this.listOrderDetail,
				listFood: this.listFood,
				listCategory: this.listCategory,
				submitOrderStatus: this.submitOrderStatus,
			},
		});

		dialogRef.afterClosed().subscribe((result) => {
			this.listOrderDetail = result.listOrderDetail;
			if (result.submitOrderStatus) {
				this.submitOrderStatus = result.submitOrderStatus;
				this.order.total = result.totalOrder;

				console.log(this.order);
				this.orderService.create(this.order).subscribe({
					next: (res) => {
						if (res.isSuccess) {
							this.order = res.data;

							this.listOrderDetail.forEach((e) => {
								e.idOrder = this.order.id;
							});
							this.toastr.success(res.message, "Thông báo", {
								timeOut: 2500,
							});

							this.orderDetailService
								.create(this.listOrderDetail)
								.subscribe({
									next: (res) => {
										if (res.isSuccess) {
											console.log(res);
										}
									},
								});
						}
					},
				});
			}
		});
	}
}
