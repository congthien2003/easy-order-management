import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { AuthenticationService } from "src/app/core/services/auth/authentication.service";
import { FormsModule } from "@angular/forms";
import { User } from "src/app/core/models/interfaces/User";

@Component({
	selector: "app-restore-password",
	templateUrl: "./restore-password.component.html",
	styleUrls: ["./restore-password.component.scss"],
	standalone: true,
	imports: [FormsModule],
})
export class RestorePasswordComponent {
	constructor(
		private service: AuthenticationService,
		private toastr: ToastrService,
		private route: Router
	) {}
	user!: User;
	restore() {
		this.service.restorePassword(this.user.email).subscribe({
			next: (res) => {
				this.toastr.success(
					"Vui lòng kiểm tra địa chỉ email.",
					"Mật khẩu đã được khôi phục",
					{
						timeOut: 5000,
					}
				);
				localStorage.setItem("token", res.data.token);
				this.route.navigate(["/pages"]);
			},
			error: (error) => {
				this.toastr.error(
					error.error.errors[0],
					"Khôi phục không thành công !",
					{
						timeOut: 3000,
					}
				);
			},
		});
	}
}
