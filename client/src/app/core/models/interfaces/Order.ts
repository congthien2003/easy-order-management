export interface Order {
	id: number;
	total: number;
	status: boolean;
	createdAt: Date;
	idTable: number;
	idStore: number;
}
