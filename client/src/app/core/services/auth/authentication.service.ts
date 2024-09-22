import { Injectable } from "@angular/core";
import { MasterService } from "../master/master.service";
import { AuthApi } from "src/app/@auth/auth.api";
import { Observable } from "rxjs";
import * as jwtdecode from "jwt-decode";
import { User } from "../../models/interfaces/User";
import { ApiResponse } from "../../models/interfaces/Common/ApiResponse";
import { JwtManager } from "../../utils/JwtManager";
@Injectable({
	providedIn: "root",
})
export class AuthenticationService {
	private readonly apiController = AuthApi;
	private jwtManager: JwtManager;
	constructor(private service: MasterService) {
		this.jwtManager = new JwtManager();
	}

	isAuthenticated(): boolean {
		const token = this.jwtManager.getToken();
		if (!token) {
			return false;
		}
		const tokendecode = jwtdecode.jwtDecode(token);
		const currentTime = Date.now() / 1000;
		return tokendecode.exp! > currentTime;
	}

	getUsernameFromToken(): any {
		const token = this.jwtManager.getToken();
		if (token === "") {
			return null;
		}
		const tokenPayload = jwtdecode.jwtDecode(token);
		if ("Username" in tokenPayload) {
			return tokenPayload["Username"];
		} else {
			return null;
		}
	}

	getIdFromToken(): any {
		const token = this.jwtManager.getToken();
		if (token === "") {
			return null;
		}
		const tokenPayload = jwtdecode.jwtDecode(token);
		if ("Id" in tokenPayload) {
			return tokenPayload["Id"];
		} else {
			return null;
		}
	}

	getInfoToken() {
		const token = this.jwtManager.getToken();
		if (token === "") {
			return null;
		}
		const tokendecode = jwtdecode.jwtDecode(token);
		return tokendecode;
	}

	refeshToken(value: User): Observable<any> {
		return this.service.post(`${this.apiController.refreshToken}`, value);
	}

	login(email: string, password: string): Observable<ApiResponse> {
		return this.service.post(`${this.apiController.login}`, {
			email,
			password,
		});
	}

	logout(): void {
		localStorage.clear();
	}

	register(
		username: string,
		email: string,
		password: string,
		phones: string,
		role: number
	): Observable<ApiResponse> {
		return this.service.post(`${this.apiController.register}`, {
			username,
			email,
			password,
			phones,
			role,
		});
	}

	changePassword(
		email: string,
		oldpassword: string,
		password: string,
		confirmPassword: string
	): Observable<ApiResponse> {
		return this.service.post(`${this.apiController.changePassword}`, {
			email,
			oldpassword,
			password,
			confirmPassword,
		});
	}

	restorePassword(email: string): Observable<ApiResponse> {
		return this.service.post(`${this.apiController.restorePassword}`, {
			email,
		});
	}
}
