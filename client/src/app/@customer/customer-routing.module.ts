import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { OrderComponent } from "./components/order/order.component";

const routes: Routes = [
	{
		path: "order/:idStore/:idTable",
		component: OrderComponent,
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class CustomerRoutingModule {}
