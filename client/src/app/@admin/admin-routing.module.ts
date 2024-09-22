import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule, Routes } from "@angular/router";
import { AdminComponent } from "./admin.component";
import { AdminDashboardComponent } from "./components/admin-dashboard/admin-dashboard.component";
import { UserComponent } from "./components/management/user/user.component";
import { StoreComponent } from "./components/management/store/store.component";
const routes: Routes = [
	{
		path: "",
		component: AdminComponent,
		children: [
			{
				path: "dashboard",
				component: AdminDashboardComponent,
			},
			{
				path: "user",
				component: UserComponent,
			},
			{
				path: "store",
				component: StoreComponent,
			},
		],
	},
	{ path: "", redirectTo: "admin", pathMatch: "full" },
	{ path: "**", redirectTo: "admin" },
];
@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AdminRoutingModule {}
