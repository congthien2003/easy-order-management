import { Food } from "../Food";

export interface OrderDetailResponse {
	quantity: number;
	food: Food;
	total: number;
}
