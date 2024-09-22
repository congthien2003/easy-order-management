import { Component, Inject } from "@angular/core";
import { CommonModule } from "@angular/common";

import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ToastrService } from "ngx-toastr";
import { NzFormModule } from "ng-zorro-antd/form";
import {
	ReactiveFormsModule,
	FormsModule,
	NonNullableFormBuilder,
	FormGroup,
	Validators,
} from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { CategoryService } from "src/app/core/services/store/category.service";
import { Category } from "src/app/core/models/interfaces/Category";

const NzModule = [NzFormModule];

@Component({
	selector: "form-edit-category",
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
export class FormEditComponent {
	validateForm!: FormGroup;
	constructor(
		public dialogRef: MatDialogRef<FormEditComponent>,
		@Inject(MAT_DIALOG_DATA) public data: { id: number },
		private fb: NonNullableFormBuilder,
		private categoryService: CategoryService,
		private toastr: ToastrService
	) {
		this.validateForm = this.fb.group({
			id: [this.data.id],
			name: ["", [Validators.required]],
			idStore: [0, [Validators.required]],
		});

		this.loadCategory();
	}

	loadCategory(): void {
		this.categoryService.getById(this.data.id).subscribe({
			next: (res) => {
				const category = res.data as Category;
				this.validateForm.setValue({
					id: category.id,
					name: category.name,
					idStore: category.idStore,
				});
			},
		});
	}

	onNoClick(): void {
		this.dialogRef.close(false);
	}

	onSubmit(): void {
		this.categoryService
			.update(this.validateForm.get("id")?.value, this.validateForm.value)
			.subscribe({
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
