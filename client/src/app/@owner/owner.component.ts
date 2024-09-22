import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";

import { MatButtonModule } from "@angular/material/button";
import { MatCommonModule } from "@angular/material/core";
import { MatMenuModule } from "@angular/material/menu";
import { Router, RouterOutlet } from "@angular/router";
import { NotfoundStoreComponent } from "./components/notfound-store/notfound-store.component";
import { AuthenticationService } from "../core/services/auth/authentication.service";
import { UserService } from "../core/services/user/user.service";
import { StoreService } from "../core/services/store/store.service";
import { ToastrService } from "ngx-toastr";
import { SpinnerComponent } from "../shared/components/spinner/spinner.component";
import { NotficationService } from "../core/services/notfication.service";
import { Store } from "../core/models/interfaces/Store";
const MatModuleImport = [MatButtonModule, MatCommonModule, MatMenuModule];

@Component({
	selector: "app-owner",
	standalone: true,
	imports: [
		CommonModule,
		MatModuleImport,
		RouterOutlet,
		NotfoundStoreComponent,
		SpinnerComponent,
	],
	templateUrl: "./owner.component.html",
	styleUrls: ["./owner.component.scss"],
})
export class OwnerComponent implements OnInit {
	currentURL: string = "dashboard";
	activeIndex: number = 0;
	idUser: number = 0;

	haveStore: boolean = false;
	store!: Store;
	constructor(
		private router: Router,
		private authService: AuthenticationService,
		private userService: UserService,
		private storeService: StoreService,
		private toastr: ToastrService,
		private notification: NotficationService
	) {
		const savedIndex = sessionStorage.getItem("selectedNavIndex");
		if (savedIndex !== null) {
			this.activeIndex = parseInt(savedIndex, 10);
		}
	}

	ngOnInit(): void {
		this.idUser = this.authService.getIdFromToken();
		this.loadStore();
	}

	loadStore(): void {
		const dataStore = sessionStorage.getItem("storeInfo");

		if (dataStore) {
			this.haveStore = true;
			this.store = JSON.parse(dataStore);
		} else {
			this.storeService.getByIdUser(this.idUser).subscribe({
				next: (res) => {
					if (res.isSuccess) {
						this.haveStore = true;
						this.store = res.data;

						sessionStorage.setItem(
							"storeInfo",
							JSON.stringify(res.data)
						);
					} else {
						this.toastr.warning(res.message, "Thông báo", {
							timeOut: 3000,
						});
					}
				},
			});
		}
	}

	swithRoute(index: number) {
		this.activeIndex = index;
		this.statusNavbar = false;
		sessionStorage.setItem("selectedNavIndex", index.toString());
		switch (index) {
			case 0: {
				this.router.navigate(["/owner/dashboard"]);
				break;
			}
			case 1: {
				this.router.navigate(["/owner/my-store"]);
				break;
			}
			case 2: {
				this.router.navigate(["/owner/table"]);
				break;
			}
			case 3: {
				this.router.navigate(["/owner/category"]);
				break;
			}
			case 4: {
				this.router.navigate(["/owner/food"]);
				break;
			}
			case 5: {
				this.router.navigate(["/owner/order"]);
				break;
			}
			case 6: {
				this.router.navigate(["/owner/invoice"]);
				break;
			}
			case 7: {
				this.router.navigate(["/owner/staff"]);
				break;
			}
			case 8: {
				this.router.navigate(["/owner/analytics"]);
				break;
			}
		}
	}

	statusNavbar: boolean = false;
	openNavbar(): void {
		this.statusNavbar = !this.statusNavbar;
	}

	bindActiveMenu(url: string) {
		switch (url) {
			case "/pages": {
				this.activeIndex = 0;
				break;
			}
			case "/pages/create": {
				this.activeIndex = 1;
				break;
			}
			case "/pages/find": {
				this.activeIndex = 2;
				break;
			}
			default: {
				this.activeIndex = -1;
				break;
			}
		}
	}

	logOut(): void {
		this.router.navigate(["/auth/login"]);
		localStorage.clear();
	}
}
