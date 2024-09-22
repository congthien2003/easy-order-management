import { Component, Inject, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";

import {
	FormGroup,
	NonNullableFormBuilder,
	Validators,
	ReactiveFormsModule,
	FormsModule,
} from "@angular/forms";
import { NzFormModule } from "ng-zorro-antd/form";
import { NzSelectModule } from "ng-zorro-antd/select";
import { UserService } from "src/app/core/services/user/user.service";
import { User } from "src/app/core/models/interfaces/User";
import { ToastrService } from "ngx-toastr";

const NzModule = [NzFormModule, NzSelectModule];

@Component({
	selector: "app-form-edit",
	standalone: true,
	imports: [
		CommonModule,
		MatButtonModule,
		ReactiveFormsModule,
		NzModule,
		FormsModule,
	],
	templateUrl: "./form-edit.component.html",
	styleUrls: ["./form-edit.component.scss"],
})
export class FormEditComponent implements OnInit {
	formAdd: boolean = true;

	validateForm!: FormGroup;

	selectedValue: number = 0;

	constructor(
		// contructor dialog
		public dialogRef: MatDialogRef<FormEditComponent>,
		@Inject(MAT_DIALOG_DATA) public data: { id: number },
		private fb: NonNullableFormBuilder,
		private userService: UserService,
		private toastr: ToastrService
	) {
		this.validateForm = this.fb.group({
			id: [this.data.id],
			username: ["", [Validators.required, Validators.minLength(6)]],
			email: ["", [Validators.email, Validators.required]],
			phones: ["", [Validators.required]],
			role: [1, [Validators.required]],
		});
	}

	ngOnInit(): void {
		if (this.data.id != undefined) {
			this.formAdd = false;
			console.log("Data", this.data.id);
			this.loadForm();
		}
	}

	loadForm(): void {
		this.userService.getById(this.data.id).subscribe({
			next: (res) => {
				const user = res.data as User;
				this.validateForm.setValue({
					id: user.id,
					username: user.username,
					email: user.email,
					phones: user.phones,
					role: user.role,
				});
			},
		});
	}

	onNoClick(): void {
		this.dialogRef.close(false);
	}

	onSubmit(): void {
		this.userService.update(this.validateForm.value).subscribe({
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
				console.log(err);

				this.toastr.error(err.message.message, "Thất bại", {
					timeOut: 3000,
				});
				this.dialogRef.close(false);
			},
		});
	}
}
