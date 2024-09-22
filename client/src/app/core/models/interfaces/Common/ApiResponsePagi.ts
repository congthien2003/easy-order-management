import { ApiResponse } from "./Common/ApiResponse";
import { Pagination } from "./Pagination";

export interface ApiResponsePagi {
	data: Pagination;
	success: boolean;
	message: string;
}
