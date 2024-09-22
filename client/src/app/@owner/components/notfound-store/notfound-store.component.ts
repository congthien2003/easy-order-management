import { Component, Input } from "@angular/core";
import { CommonModule } from "@angular/common";

import { MatButtonModule } from "@angular/material/button";
import { FormCreateStoreComponent } from "../form-create-store/form-create-store.component";
import { StoreService } from "src/app/core/services/store/store.service";
import { ToastrService } from "ngx-toastr";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { AuthenticationService } from "src/app/core/services/auth/authentication.service";
import { Router } from "@angular/router";

const MatImport = [MatButtonModule, MatDialogModule];

@Component({
	selector: "notfound-store",
	standalone: true,
	imports: [CommonModule, MatImport],
	templateUrl: "./notfound-store.component.html",
	styleUrls: ["./notfound-store.component.scss"],
})
export class NotfoundStoreComponent {
	idUser: number = 0;
	constructor(
		public dialog: MatDialog,
		private router: Router,
		private authService: AuthenticationService
	) {
		this.idUser = this.authService.getIdFromToken();
	}
	openAddDialog(): void {
		const dialogRef = this.dialog.open(FormCreateStoreComponent, {
			data: { idUser: this.idUser },
		});
		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				window.location.reload();
			}
		});
	}
}
