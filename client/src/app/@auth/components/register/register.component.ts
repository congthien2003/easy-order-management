import { Component } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { AuthenticationService } from "src/app/core/services/auth/authentication.service";
import { FormsModule } from "@angular/forms";
import { User } from "src/app/core/models/interfaces/User";
import { JwtManager } from "src/app/core/utils/JwtManager";

@Component({
	selector: "app-register",
	templateUrl: "./register.component.html",
	styleUrls: ["./register.component.scss"],
	standalone: true,
	imports: [FormsModule],
})
export class RegisterComponent {
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
		private toastr: ToastrService
	) {
		this.jwtManager = new JwtManager();
	}
	regiser() {
		// this.service
		// 	.register(this.user.username, this.user.email, this.user.password)
		// 	.subscribe({
		// 		next: (res) => {
		// 			this.jwtManager.setToken(res.data.token);
		// 			this.toastr.error("", "Đăng ký thành công !", {
		// 				timeOut: 3000,
		// 			});
		// 		},
		// 		error: (error) => {
		// 			this.toastr.error(
		// 				error.error.errors[0],
		// 				"Đăng ký không thành công !",
		// 				{
		// 					timeOut: 3000,
		// 				}
		// 			);
		// 		},
		// 	});
	}
}
