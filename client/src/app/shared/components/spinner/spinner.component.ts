import { AfterViewInit, Component } from "@angular/core";
import { NgIf } from "@angular/common";
import { LoaderService } from "src/app/core/services/loader.service";

@Component({
	selector: "app-spinner",
	templateUrl: "./spinner.component.html",
	styleUrls: ["./spinner.component.scss"],
	standalone: true,
	imports: [NgIf],
})
export class SpinnerComponent {
	constructor(public loader: LoaderService) {}
}
