import { Component } from "@angular/core";
import { NgxSpinnerService } from "ngx-spinner";
import { RouterOutlet } from "@angular/router";

@Component({
    selector: "app-pages",
    templateUrl: "./pages.component.html",
    styleUrls: ["./pages.component.scss"],
    standalone: true,
    imports: [RouterOutlet],
})
export class PagesComponent {}
