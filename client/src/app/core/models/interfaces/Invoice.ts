export interface Invoice {
	id: number;
	createdAt: Date;
	finishedAt: Date;
	status: boolean;
	totalOrder: number;
	charge: number;
	total: number;
	idOrder: number;
	idPaymentType: number;
}
