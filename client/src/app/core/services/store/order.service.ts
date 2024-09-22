import { Injectable } from "@angular/core";
import { OrderApi } from "../../constant/api/order.api";
import { HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../models/interfaces/Common/ApiResponse";
import { Pagination } from "../../models/interfaces/Common/Pagination";
import { MasterService } from "../master/master.service";
import { Order } from "../../models/interfaces/Order";

@Injectable({
	providedIn: "root",
})
export class OrderService {
	endpoint = OrderApi;
	constructor(private service: MasterService) {}

	list(
		idStore: number,
		pagi: Pagination,
		filter: boolean = false,
		status: boolean = false
	): Observable<ApiResponse> {
		const params = new HttpParams()
			.set("idStore", idStore)
			.set("currentPage", pagi.currentPage)
			.set("pageSize", pagi.pageSize)
			.set("filter", filter)
			.set("status", status);

		return this.service.get(this.endpoint.getAll, { params });
	}

	getById(id: number): Observable<ApiResponse> {
		return this.service.get(`${this.endpoint.getById}/${id} `);
	}

	create(order: Order): Observable<ApiResponse> {
		return this.service.post(`${this.endpoint.create}`, order);
	}

	update(id: number, order: Order): Observable<ApiResponse> {
		return this.service.put(`${this.endpoint.update}/${id}`, order);
	}

	deleteById(id: number): Observable<ApiResponse> {
		return this.service.delete(`${this.endpoint.delete}/${id}`);
	}

	aceept(id: number): Observable<ApiResponse> {
		return this.service.get(`${this.endpoint.accept}/${id}`);
	}
}
