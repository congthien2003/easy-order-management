import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiResponse } from "../../models/interfaces/Common/ApiResponse";
import { Pagination } from "../../models/interfaces/Common/Pagination";
import { MasterService } from "../master/master.service";
import { OrderDetailApi } from "../../constant/api/orderDetail.api";
import { OrderDetail } from "../../models/interfaces/OrderDetail";

@Injectable({
	providedIn: "root",
})
export class OrderDetailService {
	endpoint = OrderDetailApi;
	constructor(private service: MasterService) {}

	list(
		idOrder: number,
		pagi: Pagination,
		filter: boolean = false,
		status: boolean = false
	): Observable<ApiResponse> {
		const params = new HttpParams()
			.set("currentPage", pagi.currentPage)
			.set("pageSize", pagi.pageSize)
			.set("filter", filter)
			.set("status", status);

		return this.service.get(`${this.endpoint.getByIdOrder}/${idOrder}`, {
			params,
		});
	}

	create(orderDetail: OrderDetail[]): Observable<ApiResponse> {
		return this.service.post(`${this.endpoint.create}`, orderDetail);
	}

	update(id: number, orderDetail: OrderDetail): Observable<ApiResponse> {
		return this.service.put(`${this.endpoint.update}/${id}`, orderDetail);
	}

	deleteById(id: number): Observable<ApiResponse> {
		return this.service.delete(`${this.endpoint.delete}/${id}`);
	}
}
