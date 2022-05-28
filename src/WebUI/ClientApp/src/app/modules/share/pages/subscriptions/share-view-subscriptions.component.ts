import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';
import { WeShareClient } from '../../../../../services/weshareclient';
import { PaginatedResponse, Resolved } from '../../../../../types/general-types';
import { ShareData } from '../../../../../types/share-types';
import { SubscriptionSnippet } from '../../../../../types/subscription-types';
import { PagedListHeaderEvent } from '../../../../shared/components/paged-list-header/paged-list-header.component';
import { ShareService } from '../../services/shareservice';

@Component({
  selector: 'share-view-subscriptions',
  templateUrl: './share-view-subscriptions.component.html',
  styleUrls: ['./share-view-subscriptions.component.css']
})
export class ShareViewSubscriptionsComponent {
  errorCode: number;
  shareData: ShareData;

  subscriptionSnippetsResponse: PaginatedResponse<SubscriptionSnippet>;
  subscriptionSnippets: SubscriptionSnippet[];

  lastPageEvent: PageEvent | null;

  constructor(private weShareClient: WeShareClient, private authService: AuthService,
    shareService: ShareService, router: Router, route: ActivatedRoute) {
    if (authService.isLoggedOut()) {
      router.navigate(['/']);
      authService.requestLogin();
    }

    this.shareData = shareService.shareData;

    route.data.subscribe(data => {
      const subscriptionSnippetsResponse: Resolved<PaginatedResponse<SubscriptionSnippet>> = data.subscriptionSnippetsResponse;

      if (subscriptionSnippetsResponse.ok) {
        this.subscriptionSnippetsResponse = subscriptionSnippetsResponse.content!;
        this.subscriptionSnippets = this.subscriptionSnippetsResponse.items;
      } else {
        if (subscriptionSnippetsResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }
        if (subscriptionSnippetsResponse.status == 403) {
          router.navigateByUrl("/forbidden");
          return;
        }

        this.errorCode = subscriptionSnippetsResponse.status;
        return;
      }
    });
  }


  onPagedListHeaderEvent(pagedListHeaderEvent: PagedListHeaderEvent) {
    const pageEvent = pagedListHeaderEvent.pageEvent;
    this.refreshList(pageEvent);
  }

  refreshList(pageEvent: PageEvent | null) {
    this.lastPageEvent = pageEvent;

    if (pageEvent) {
      this.updateList(pageEvent.pageIndex, pageEvent.pageSize);
    }
    else {
      this.updateList(0, 10);
    }
  }

  updateList(pageIndex: number, pageSize: number) {
    this.weShareClient.getSubscriptionSnippets(this.authService.getUserId()!, null, pageIndex, pageSize)
      .subscribe(success => {
        this.subscriptionSnippetsResponse = success;
        this.subscriptionSnippets = this.subscriptionSnippetsResponse.items;
      }, error => {
        alert("There was an error while loading the data!");
      });
  }

  onDelete(subscriptionSnippet: SubscriptionSnippet) {
    this.weShareClient.removeSubscription(subscriptionSnippet.id)
      .subscribe(success => {
        this.refreshList(this.lastPageEvent);
      }, error => {
        this.refreshList(this.lastPageEvent);
        alert("There was an error while deleting subscription");
      });
  }

}
