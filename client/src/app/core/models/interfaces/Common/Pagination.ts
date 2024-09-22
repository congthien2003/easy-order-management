export interface Pagination {
	totalPage: number;
	totalRecords: number;
	currentPage: number;
	pageSize: number;
	hasNextPage: boolean;
	hasPrevPage: boolean;
}
