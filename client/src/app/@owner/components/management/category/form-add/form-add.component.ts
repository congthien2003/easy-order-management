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
import { CategoryService } from "src/app/core/services/store/category.service";

const NzModule = [NzFormModule, NzSelectModule];

@Component({
	selector: "form-add-category",
	standalone: true,
	imports: [
		CommonModule,
		MatButtonModule,
		ReactiveFormsModule,
		NzModule,
		FormsModule,
	],
	templateUrl: "./form-add.component.html",
	styleUrls: ["./form-add.component.scss"],
})
export class FormAddComponent {
	validateForm!: FormGroup;
	constructor(
		// contructor dialog
		public dialogRef: MatDialogRef<FormAddComponent>,
		@Inject(MAT_DIALOG_DATA)
		public data: {
			idStore: number;
		},
		private fb: NonNullableFormBuilder,
		private categoryServices: CategoryService,
		private toastr: ToastrService
	) {
		this.validateForm = this.fb.group({
			id: [0],
			name: ["", [Validators.required]],
			idStore: [this.data.idStore, [Validators.required]],
		});
	}

	onNoClick(): void {
		this.dialogRef.close(false);
	}

	onSubmit(): void {
		this.categoryServices.create(this.validateForm.value).subscribe({
			next: (res) => {
				console.log(res);
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
