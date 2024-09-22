import { Injectable } from "@angular/core";

@Injectable({
	providedIn: "root",
})
export class LoaderService {
	private loading: boolean = true;

	constructor() {
		this.loading = true;
	}

	setLoading(loading: boolean) {
		this.loading = loading;
	}

	getLoading(): boolean {
		return this.loading;
	}
}
