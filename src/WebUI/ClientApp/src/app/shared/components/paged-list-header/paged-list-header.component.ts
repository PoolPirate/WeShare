import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'shared-paged-list-header',
  templateUrl: './paged-list-header.component.html',
  styleUrls: ['./paged-list-header.component.css']
})
export class PagedListHeaderComponent implements OnInit {
  @Input()
  showCreateButton: boolean = true;

  @Input()
  showPaginator: boolean = true;

  @Input()
  showSortMenu: boolean = true;

  @Input()
  count: number;

  @Input()
  sortOptionsEnum: Object;

  @Input()
  defaultOrdering: number;

  @Output()
  create = new EventEmitter();

  @Output()
  page = new EventEmitter<PagedListHeaderEvent>();

  activeOrdering: number;

  constructor(private route: ActivatedRoute, private router: Router) {
    route.queryParams.subscribe(queryParams => {
      this.activeOrdering = parseInt(queryParams["order"]);
    });
  }
  ngOnInit(): void {

    if (this.showSortMenu && !Object.values(this.sortOptionsEnum).includes(this.activeOrdering)) {
      this.activeOrdering = this.defaultOrdering;
    }
  }

  onPage(pageEvent: PageEvent) {
    const event = { pageEvent: pageEvent, ordering: this.activeOrdering };
    this.page.emit(event);
  }

  onCreate() {
    this.create.emit();
  }

  applyOrdering(orderingName: string) {
    this.activeOrdering = this.orderingNameToId(orderingName);
    this.router.navigate(['.'], {
      relativeTo: this.route,
      queryParamsHandling: 'merge',
      queryParams: { order: this.activeOrdering },
    });

    this.page.emit({ pageEvent: null, ordering: this.activeOrdering })
  }

  orderingNameToId(orderingName: string) {
    const orderingId = parseInt(Object.entries(this.orderingOptions).find(x => x[1] == orderingName)![0]);
    return orderingId;
  }

  get requireBetween() {
    return (this.showCreateButton && this.showPaginator);
  }

  get requireEnd() {
    return !(this.showCreateButton && this.showPaginator);
  }

  get orderingOptions() {
    const values = Object.values(this.sortOptionsEnum);
    return values.slice(0, values.length / 2);
  }
}

export interface PagedListHeaderEvent {
  pageEvent: PageEvent | null;
  ordering: number;
}
