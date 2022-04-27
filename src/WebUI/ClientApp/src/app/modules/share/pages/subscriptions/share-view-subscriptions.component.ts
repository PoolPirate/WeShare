import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';
import { PaginatedResponse, Resolved } from '../../../../../types/general-types';
import { ShareData } from '../../../../../types/share-types';
import { SubscriptionSnippet } from '../../../../../types/subscription-types';
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

  constructor(shareService: ShareService, authService: AuthService, router: Router, route: ActivatedRoute) {
    if (authService.isLoggedOut()) {
      router.navigate(['login']);
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
}
