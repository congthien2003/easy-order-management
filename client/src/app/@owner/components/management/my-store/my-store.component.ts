import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AuthenticationService } from "src/app/core/services/auth/authentication.service";
import { StoreService } from "src/app/core/services/store/store.service";
import { Store } from "src/app/core/models/interfaces/Store";
import { FormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";

@Component({
	selector: "app-my-store",
	standalone: true,
	imports: [CommonModule, FormsModule, MatButtonModule],
	templateUrl: "./my-store.component.html",
	styleUrls: ["./my-store.component.scss"],
})
export class MyStoreComponent implements OnInit {
	idUser: number = 0;
	idStore: number = 0;
	store!: Store;

	isEditing = false; // Biến để điều khiển chế độ chỉnh sửa

	constructor(
		private authService: AuthenticationService,
		private storeService: StoreService
	) {}

	ngOnInit(): void {
		this.idUser = this.authService.getIdFromToken();
		this.store = JSON.parse(
			sessionStorage.getItem("storeInfo") ?? ""
		) as Store;
	}

	// Hàm để bật/tắt chế độ chỉnh sửa
	toggleEdit() {
		this.isEditing = !this.isEditing;
	}
}
