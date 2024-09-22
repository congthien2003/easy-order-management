import { Component, Inject } from "@angular/core";
import { CommonModule } from "@angular/common";
import {
	AbstractControl,
	FormGroup,
	FormsModule,
	NonNullableFormBuilder,
	ReactiveFormsModule,
	ValidatorFn,
	Validators,
} from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ToastrService } from "ngx-toastr";
import { User } from "src/app/core/models/interfaces/User";
import { UserService } from "src/app/core/services/user/user.service";
import { MatButtonModule } from "@angular/material/button";

// Import NZ
import { NzFormModule } from "ng-zorro-antd/form";
import { NzSelectModule } from "ng-zorro-antd/select";
import { AuthenticationService } from "src/app/core/services/auth/authentication.service";
import { StoreService } from "src/app/core/services/store/store.service";

const NzModule = [NzFormModule, NzSelectModule];
@Component({
	selector: "app-form-create-store",
	standalone: true,
	imports: [
		CommonModule,
		MatButtonModule,
		ReactiveFormsModule,
		NzModule,
		FormsModule,
	],
	templateUrl: "./form-create-store.component.html",
	styleUrls: ["./form-create-store.component.scss"],
})
export class FormCreateStoreComponent {
	validateForm!: FormGroup;
	selectedValue: number = 0;

	constructor(
		// contructor dialog
		public dialogRef: MatDialogRef<FormCreateStoreComponent>,
		@Inject(MAT_DIALOG_DATA) public data: { idUser: number },
		private fb: NonNullableFormBuilder,
		private toastr: ToastrService,
		private storeService: StoreService
	) {
		this.validateForm = this.fb.group({
			id: [0],
			name: ["", [Validators.required, Validators.minLength(6)]],
			address: ["", [Validators.required]],
			phone: ["", [Validators.required]],
			idUser: [this.data.idUser, [Validators.required]],
		});
	}

	onNoClick(): void {
		this.dialogRef.close(false);
	}

	onSubmit(): void {
		this.storeService.create(this.validateForm.value).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.toastr.success(res.message, "Thành công", {
						timeOut: 3000,
					});
					this.dialogRef.close(true);
				} else {
					this.toastr.error(res.message, "Thất bại", {
						timeOut: 3000,
					});
					this.dialogRef.close(false);
				}
			},
			error: (err) => {
				this.toastr.error(err.message, "Thất bại", {
					timeOut: 3000,
				});
				this.dialogRef.close(false);
			},
		});
	}
}
