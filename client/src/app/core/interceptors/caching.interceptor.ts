import { Injectable } from "@angular/core";
import {
	HttpRequest,
	HttpHandler,
	HttpEvent,
	HttpInterceptor,
	HttpResponse,
} from "@angular/common/http";
import { Observable, of, tap } from "rxjs";
import { CachingService } from "../services/caching.service";

@Injectable()
export class CachingInterceptor implements HttpInterceptor {
	constructor(private cacheService: CachingService) {}

	intercept(
		request: HttpRequest<unknown>,
		next: HttpHandler
	): Observable<HttpEvent<unknown>> {
		if (request.method !== "GET") {
			return next.handle(request);
		}
		const cachedResponse = this.cacheService.get(request.url);
		if (cachedResponse) {
			return of(cachedResponse);
		}

		return next.handle(request).pipe(
			tap((event) => {
				if (event instanceof HttpResponse) {
					this.cacheService.set(request.url, event);
				}
			})
		);
	}
}
