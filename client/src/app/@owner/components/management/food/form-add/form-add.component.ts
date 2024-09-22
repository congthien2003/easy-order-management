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

import { CategoryService } from "src/app/core/services/store/category.service";

// Import NZ
import { NzFormModule } from "ng-zorro-antd/form";
import { NzSelectModule } from "ng-zorro-antd/select";
import { Category } from "src/app/core/models/interfaces/Category";
import { FoodService } from "src/app/core/services/store/food.service";
import { FirebaseService } from "src/app/core/services/firebase.service";
const NzModule = [NzFormModule, NzSelectModule];

@Component({
	selector: "form-add-food",
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
	listCategory!: Category[];
	selectedCategoryId: string = "";
	validateForm!: FormGroup;
	constructor(
		// contructor dialog
		public dialogRef: MatDialogRef<FormAddComponent>,
		@Inject(MAT_DIALOG_DATA) public data: { idStore: number },

		private fb: NonNullableFormBuilder,
		private categoryServices: CategoryService,
		private foodService: FoodService,
		private firebaseService: FirebaseService,
		private toastr: ToastrService
	) {
		this.validateForm = this.fb.group({
			id: [0],
			name: ["", [Validators.required]],
			price: [0, [Validators.required]],
			idCategory: [0, [Validators.required]],
			status: [Boolean],
		});

		this.categoryServices.getAllByIdStore(this.data.idStore).subscribe({
			next: (res) => {
				this.listCategory = res.data;
				console.log(res);
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

	onSubmit(): void {
		console.log(this.fileImage);
		if (this.fileImage) {
			this.firebaseService.uploadFileImage(this.fileImage).subscribe(
				(imageUrl: string) => {
					this.onCreateFood(imageUrl);
				},
				(error) => {
					console.error("Lỗi upload file:", error);
				}
			);
		}
	}

	onCreateFood(imageUrl: string) {
		const data = { ...this.validateForm.value, imageUrl };
		this.foodService.create(data).subscribe({
			next: (res) => {
				if (res.isSuccess) {
					this.toastr.success(res.message, "Thành công", {
						timeOut: 3000,
					});
					this.dialogRef.close(res.data);
				} else {
					this.toastr.error(res.message, "Thất bại", {
						timeOut: 3000,
					});
					this.dialogRef.close(null);
				}
			},
			error: (err) => {
				this.toastr.error(err.error.message, "Thất bại", {
					timeOut: 3000,
				});
				this.dialogRef.close(null);
			},
		});
	}
}
