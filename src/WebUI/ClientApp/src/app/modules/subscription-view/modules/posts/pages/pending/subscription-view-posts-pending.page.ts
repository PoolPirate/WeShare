import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResponse, Resolved } from '../../../../../../../types/general-types';
import { PostSendInfo } from '../../../../../../../types/post-types';
import { SubscriptionViewService } from '../../../../services/subscriptionviewservice';

@Component({
  selector: 'subscription-view-posts-pending',
  templateUrl: './subscription-view-posts-pending.page.html'
})
export class SubscriptionViewPostsPendingPage {
  pendingPostsResponse: PaginatedResponse<PostSendInfo>;
  pendingPosts: PostSendInfo[];

  errorCode: number;

  constructor(private subscriptionViewService: SubscriptionViewService, route: ActivatedRoute, router: Router) {
    route.data.subscribe(data => {
      const pendingResponse: Resolved<PaginatedResponse<PostSendInfo>> = data.pendingResponse;
      console.log(pendingResponse);
      if (pendingResponse.ok) {
        this.pendingPostsResponse = pendingResponse.content!;
        this.pendingPosts = this.pendingPostsResponse.items;
      } else {
        if (pendingResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }

        this.errorCode = pendingResponse.status;
        return;
      }
    });
  }

  get subscriptionType() {
    return this.subscriptionViewService.subscriptionInfo.type;
  }
}
