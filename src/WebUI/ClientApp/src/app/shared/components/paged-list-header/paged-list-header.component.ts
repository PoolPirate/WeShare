import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'shared-paged-list-header',
  templateUrl: './paged-list-header.component.html',
  styleUrls: ['./paged-list-header.component.css']
})
export class PagedListHeaderComponent {
  @Input()
  showCreateButton: boolean = true;

  @Input()
  showPaginator: boolean = true;

  @Input()
  count: number;

  @Output()
  create = new EventEmitter();

  @Output()
  page = new EventEmitter<PageEvent>();

  onPage(pageEvent: PageEvent) {
    this.page.emit(pageEvent);
  }

  onCreate() {
    this.create.emit();
  }

  get requireBetween() {
    return (this.showCreateButton && this.showPaginator);
  }

  get requireEnd() {
    return !(this.showCreateButton && this.showPaginator);
  }
}
