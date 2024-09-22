import { Food } from "./Food";

export interface CartItem {
	Food: Food | undefined;
	quantity: number;
	price: number;
}
