import { Injectable } from "@angular/core";
import { HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../models/interfaces/Common/ApiResponse";
import { Pagination } from "../../models/interfaces/Common/Pagination";
import { MasterService } from "../master/master.service";
import { StoreApi } from "../../constant/api/store.api";

@Injectable({
	providedIn: "root",
})
export class StoreService {
	endpoint = StoreApi;
	constructor(private service: MasterService) {}

	list(
		idStore: number,
		pagi: Pagination,
		searchTerm: string = "",
		sortColumn: string = "",
		ascSort: boolean = true
	): Observable<ApiResponse> {
		const params = new HttpParams()
			.set("idStore", idStore)
			.set("currentPage", pagi.currentPage)
			.set("pageSize", pagi.pageSize)
			.set("searchTerm", searchTerm)
			.set("sortColumn", sortColumn)
			.set("ascSrot", ascSort);
		return this.service.get(this.endpoint.getAll, { params });
	}

	getById(id: number): Observable<ApiResponse> {
		return this.service.get(`${this.endpoint.getById}/${id} `);
	}

	getByIdUser(id: number): Observable<ApiResponse> {
		return this.service.get(`${this.endpoint.getByIdUser}/${id} `);
	}

	create(user: any): Observable<ApiResponse> {
		return this.service.post(`${this.endpoint.create}`, user);
	}

	update(id: number, user: any): Observable<ApiResponse> {
		return this.service.put(`${this.endpoint.update}/${id}`, user);
	}

	deleteById(id: number): Observable<ApiResponse> {
		const params = new HttpParams().set("id", id);
		return this.service.delete(`${this.endpoint.delete}/`, { params });
	}
}
