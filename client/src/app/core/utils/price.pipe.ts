import { inject, Inject, Pipe, PipeTransform } from "@angular/core";
import { CategoryService } from "../services/store/category.service";
import { map } from "rxjs";

@Pipe({
	name: "price",
	standalone: true,
})
export class PricePipe implements PipeTransform {
	transform(price: number) {
		return price.toLocaleString("vi", {
			style: "currency",
			currency: "VND",
		});
	}
}
