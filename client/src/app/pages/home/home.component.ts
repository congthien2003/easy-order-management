import { Component, OnInit } from "@angular/core";
import { NgxSpinnerService } from "ngx-spinner";
import { FooterComponent } from "../layout/footer/footer.component";
import { NgFor } from "@angular/common";
import { HeaderComponent } from "../layout/header/header.component";

@Component({
	selector: "app-home",
	templateUrl: "./home.component.html",
	styleUrls: ["./home.component.scss"],
	standalone: true,
	imports: [HeaderComponent, NgFor, FooterComponent],
})
export class HomeComponent {
	constructor() {}
}
