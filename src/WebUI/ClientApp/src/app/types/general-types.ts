import { HttpErrorResponse } from "@angular/common/http";

export class PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  totalCount: number;
  totalPages: number;
}

export class Resolved<T> {
  content: T | null;
  status: number;
  ok: boolean;

  public static success<T>(content: T) {
    return new Resolved<T>(content, 200, true);
  }
  public static error<T>(error: HttpErrorResponse) {
    return new Resolved<T>(null, error.status, false);
  }

  constructor(content: T | null, status: number, ok: boolean) {
    this.content = content;
    this.status = status;
    this.ok = ok;
  }
}
