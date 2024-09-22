import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
	name: "roleName",
	standalone: true,
})
export class RolePipe implements PipeTransform {
	transform(id: number) {
		return id === 0 ? "Admin" : "Chủ quán";
	}
}
