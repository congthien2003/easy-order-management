import { inject, Inject, Pipe, PipeTransform } from "@angular/core";
import { CategoryService } from "../services/store/category.service";
import { map } from "rxjs";

@Pipe({
	name: "categoryName",
	standalone: true,
})
export class CategoryPipe implements PipeTransform {
	private service = inject(CategoryService);

	transform(id: number) {
		return this.service.getById(id).pipe(
			map((res) => {
				return res.data.name;
			})
		);
	}
}
