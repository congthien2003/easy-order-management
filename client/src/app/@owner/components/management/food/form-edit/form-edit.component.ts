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
import { FoodService } from "src/app/core/services/store/food.service";
import { Food } from "src/app/core/models/interfaces/Food";
import { FirebaseService } from "src/app/core/services/firebase.service";

const NzModule = [NzFormModule];

@Component({
	selector: "form-edit-food",
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
	food!: Food;
	validateForm!: FormGroup;
	constructor(
		public dialogRef: MatDialogRef<FormEditComponent>,
		@Inject(MAT_DIALOG_DATA)
		public data: { id: number; listCategory: Category[] },
		private fb: NonNullableFormBuilder,
		private categoryService: CategoryService,
		private foodService: FoodService,
		private toastr: ToastrService,
		private firebase: FirebaseService
	) {
		this.validateForm = this.fb.group({
			id: [0],
			name: ["", [Validators.required]],
			price: [0, [Validators.required]],
			idCategory: [0, [Validators.required]],
			status: [Boolean],
		});
		this.loadFood();
	}

	loadFood(): void {
		this.foodService.getById(this.data.id).subscribe({
			next: (res) => {
				console.log(res);
				this.food = res.data;

				this.validateForm.setValue({
					id: this.food.id,
					name: this.food.name,
					price: this.food.price,
					idCategory: this.food.idCategory,
					status: this.food.status,
				});
			},
		});
	}

	fileImage!: File;
	handleChange($event: any): void {
		// imageUrl
		this.fileImage = $event.target.files[0];
	}

	onNoClick(): void {
		this.dialogRef.close(false);
	}

	async onSubmit(): Promise<void> {
		if (this.fileImage) {
			await this.firebase.deleteFileFromFirebase(this.food.imageUrl);

			this.firebase.uploadFileImage(this.fileImage).subscribe({
				next: (imageUrl: string) => {
					const dataEdit = { ...this.validateForm.value, imageUrl };
					this.foodService
						.update(this.validateForm.get("id")?.value, dataEdit)
						.subscribe({
							next: (res) => {
								if (res.isSuccess) {
									this.toastr.success(
										res.message,
										"Thành công",
										{
											timeOut: 3000,
										}
									);
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

								this.toastr.error(
									err.message.message,
									"Thất bại",
									{
										timeOut: 3000,
									}
								);
								this.dialogRef.close(false);
							},
						});
				},
				error: () => {
					this.toastr.error("Có lỗi xảy ra", "Thông báo", {
						timeOut: 3000,
					});
				},
			});
		}
	}
}
