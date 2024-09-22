import { Injectable } from "@angular/core";
import { PaymentApi } from "../../constant/api/payment.api";
import { HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../models/interfaces/Common/ApiResponse";
import { Pagination } from "../../models/interfaces/Common/Pagination";
import { Order } from "../../models/interfaces/Order";
import { MasterService } from "../master/master.service";
import { PaymentType } from "../../models/interfaces/PaymentType";

@Injectable({
	providedIn: "root",
})
export class PaymentService {
	endpoint = PaymentApi;
	constructor(private service: MasterService) {}

	list(idStore: number, pagi: Pagination): Observable<ApiResponse> {
		const params = new HttpParams()
			.set("idStore", idStore)
			.set("currentPage", pagi.currentPage)
			.set("pageSize", pagi.pageSize);

		return this.service.get(this.endpoint.getAll, { params });
	}

	getById(id: number): Observable<ApiResponse> {
		return this.service.get(`${this.endpoint.getById}/${id} `);
	}

	create(data: PaymentType): Observable<ApiResponse> {
		return this.service.post(`${this.endpoint.create}`, data);
	}

	update(id: number, data: PaymentType): Observable<ApiResponse> {
		return this.service.put(`${this.endpoint.update}/${id}`, data);
	}

	deleteById(id: number): Observable<ApiResponse> {
		return this.service.delete(`${this.endpoint.delete}/${id}`);
	}
}
