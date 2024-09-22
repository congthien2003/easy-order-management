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

const NzModule = [NzFormModule, NzSelectModule];

@Component({
	selector: "app-form-add",
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
	selectedValue: number = 0;

	constructor(
		// contructor dialog
		public dialogRef: MatDialogRef<FormAddComponent>,
		private fb: NonNullableFormBuilder,
		private userService: UserService,
		private toastr: ToastrService
	) {
		this.validateForm = this.fb.group({
			id: [0],
			username: ["", [Validators.required, Validators.minLength(6)]],
			email: ["", [Validators.email, Validators.required]],
			password: ["", [Validators.required]],
			checkPassword: [
				"",
				[Validators.required, this.confirmationValidator],
			],
			phones: ["", [Validators.required]],
			role: [1, [Validators.required]],
		});
	}

	ngOnInit(): void {
		console.log("form add");
	}

	loadForm(): void {
		// this.userService.getById(this.data.id).subscribe({
		// 	next: (res) => {
		// 		const user = res.data as User;
		// 		this.validateForm.setValue({
		// 			id: user.id,
		// 			username: user.username,
		// 			email: user.email,
		// 			phones: user.phones,
		// 			role: user.role,
		// 		});
		// 	},
		// });
	}

	onNoClick(): void {
		this.dialogRef.close(false);
	}

	onSubmit(): void {
		const data = { ...this.validateForm.value };
		delete data.checkPassword;
		this.userService.create(data).subscribe({
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

	updateConfirmValidator(): void {
		/** wait for refresh value */
		Promise.resolve().then(() =>
			this.validateForm.controls["checkPassword"].updateValueAndValidity()
		);
	}

	confirmationValidator: ValidatorFn = (
		control: AbstractControl
	): { [s: string]: boolean } => {
		if (!control.value) {
			return { required: true };
		} else if (
			control.value !== this.validateForm.controls["password"].value
		) {
			return { confirm: true, error: true };
		}
		return {};
	};
}
