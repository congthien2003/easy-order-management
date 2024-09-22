import { RouterModule, Routes, RouterOutlet } from "@angular/router";
import { NgModule } from "@angular/core";
import { HomeComponent } from "./home/home.component";
import { authGuard } from "../@auth/guards/auth.guard";
import { UserComponent } from "./user/user.component";

const routes: Routes = [
	{
		path: "",
		component: HomeComponent,
	},
	{
		path: "user",
		component: UserComponent,
		canActivate: [authGuard],
	},
	{
		path: "find",
		component: HomeComponent,
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class PagesRoutingModule {}
