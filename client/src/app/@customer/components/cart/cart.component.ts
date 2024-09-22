import { Component, Inject, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import {
	MatDialogRef,
	MAT_DIALOG_DATA,
	MatDialogModule,
} from "@angular/material/dialog";
import { OrderDetail } from "src/app/core/models/interfaces/OrderDetail";
import { Category } from "src/app/core/models/interfaces/Category";
import { Food } from "src/app/core/models/interfaces/Food";

// Mat Import
import { MatButtonModule } from "@angular/material/button";
import { CartItem } from "src/app/core/models/interfaces/CartItem";
import { FoodService } from "src/app/core/services/store/food.service";
import { map } from "rxjs";
import { PricePipe } from "src/app/core/utils/price.pipe";
const MatImport = [MatButtonModule];

@Component({
	selector: "app-cart",
	standalone: true,
	imports: [CommonModule, MatImport, PricePipe],
	templateUrl: "./cart.component.html",
	styleUrls: ["./cart.component.scss"],
})
export class CartComponent implements OnInit {
	config = {
		displayedColumns: [
			{
				prop: "id",
				display: "STT",
			},
			{
				prop: "name",
				display: "Tên món ăn",
			},
			{
				prop: "price",
				display: "Giá tiền",
			},
			{
				prop: "quantity",
				display: "Số lượng",
			},
			{
				prop: "total",
				display: "Thành tiền",
			},
		],
		hasAction: true,
	};

	listCartItem: CartItem[] = [];
	listFood!: Food[];
	totalOrder: number = 0;
	ngOnInit(): void {
		console.log(this.data);
	}
	constructor(
		private foodService: FoodService,
		private _dialogRef: MatDialogRef<CartComponent>,
		@Inject(MAT_DIALOG_DATA)
		public data: {
			listOrderDetail: OrderDetail[];
			listCategory: Category[];
			submitOrderStatus: boolean;
		}
	) {
		this.loadCartItem();
	}

	loadCartItem(): void {
		this.foodService
			.getByListId(this.data.listOrderDetail.map((e) => e.idFood))
			.subscribe({
				next: (res) => {
					this.listFood = res.data;
					this.data.listOrderDetail.forEach((e) => {
						const food = this.listFood.filter(
							(f) => f.id === e.idFood
						)[0];
						this.listCartItem.push({
							Food: food,
							quantity: e.quantity,
							price: food.price * e.quantity,
						});
					});
					this.loadTotalOrder();
				},
			});
	}

	loadTotalOrder(): void {
		this.totalOrder = 0;
		this.listCartItem
			.map((e) => e.price)
			.forEach((e) => {
				this.totalOrder += e;
			});
	}

	deleteCartItem(id: number) {
		if (id !== -1) {
			this.listCartItem = this.listCartItem.filter(
				(e) => e.Food?.id !== id
			);
			this.data.listOrderDetail = this.data.listOrderDetail.filter(
				(e) => e.idFood !== id
			);
			this.loadTotalOrder();
		}
	}

	onSubmit(): void {
		this._dialogRef.close({
			listOrderDetail: this.data.listOrderDetail,
			totalOrder: this.totalOrder,
			submitOrderStatus: true,
		});
	}

	onNoClick(): void {
		this._dialogRef.close({
			listOrderDetail: this.data.listOrderDetail,
			submitOrderStatus: false,
		});
	}
}
