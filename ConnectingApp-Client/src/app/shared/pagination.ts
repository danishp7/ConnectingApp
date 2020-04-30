export interface Pagination {
  currentPage: number;
  totalItems: number;
  itemsPerPage: number;
  totalPages: number;
}

// to store the info of resulting object and header values
export class PaginatedResult<T> {
  result: T; // to store items
  pagination: Pagination; // to store header values
}
