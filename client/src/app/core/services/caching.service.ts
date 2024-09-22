import {
	HttpEvent,
	HttpHandler,
	HttpInterceptor,
	HttpResponse,
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, of, tap } from "rxjs";

@Injectable({
	providedIn: "root",
})
export class CachingService {
	constructor() {}

	private cache = new Map<string, HttpResponse<any>>();

	get(key: string): HttpResponse<any> | undefined {
		return this.cache.get(key);
	}

	set(key: string, value: HttpResponse<any>): void {
		this.cache.set(key, value);
	}
}
