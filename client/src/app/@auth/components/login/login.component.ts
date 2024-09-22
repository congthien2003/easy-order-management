import { Component, OnInit } from "@angular/core";
import {
	FormBuilder,
	FormGroup,
	Validators,
	FormsModule,
} from "@angular/forms";
import { AuthenticationService } from "src/app/core/services/auth/authentication.service";
import { ToastrService } from "ngx-toastr";
import { animate, style, transition, trigger } from "@angular/animations";
import { Route, Router } from "@angular/router";
import { User } from "src/app/core/models/interfaces/User";
import { JwtManager } from "src/app/core/utils/JwtManager";
import { MatButtonModule } from "@angular/material/button";
@Component({
	selector: "app-login",
	templateUrl: "./login.component.html",
	styleUrls: ["./login.component.scss"],
	standalone: true,
	imports: [FormsModule, MatButtonModule],
})
export class LoginComponent implements OnInit {
	isLoading = true;
	user: User = {
		id: 0,
		username: "",
		password: "",
		email: "",
		phones: "",
		role: 1,
	};
	private jwtManager: JwtManager;
	constructor(
		private service: AuthenticationService,
		private toastr: ToastrService,
		private route: Router
	) {
		this.jwtManager = new JwtManager();
	}

	ngOnInit() {
		this.isLoading = false;
	}

	initForm() {}

	login() {
		if (this.service.isAuthenticated()) {
			this.route.navigate(["/pages"]);
		} else {
			this.service.login(this.user.email, this.user.password).subscribe({
				next: (res) => {
					this.jwtManager.setToken(res.data.token);
					this.route.navigate(["/pages"]);
				},
				error: (error) => {
					console.log(error);

					this.toastr.error(
						error.error.errors[0],
						"Đăng nhập không thành công !",
						{
							timeOut: 3000,
						}
					);
				},
			});
		}
	}
}
