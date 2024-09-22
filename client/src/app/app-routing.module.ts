import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

const routes: Routes = [
	{
		path: "auth",
		loadChildren: () =>
			import("./@auth/auth.module").then((m) => m.AuthModule),
	},
	{
		path: "pages",
		loadChildren: () =>
			import("./pages/pages.module").then((m) => m.PagesModule),
	},
	{
		path: "admin",
		loadChildren: () =>
			import("./@admin/admin.module").then((m) => m.AdminModule),
	},
	{
		path: "owner",
		loadChildren: () =>
			import("./@owner/owner.module").then((m) => m.OwnerModule),
	},
	{
		path: "customer",
		loadChildren: () =>
			import("./@customer/customer.module").then((m) => m.CustomerModule),
	},
	{ path: "", redirectTo: "pages", pathMatch: "full" },
	{ path: "**", redirectTo: "pages" },
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}
