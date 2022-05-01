import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResponse, Resolved } from '../../../../../../../types/general-types';
import { PostSendInfo } from '../../../../../../../types/post-types';

@Component({
  selector: 'subscription-view-posts-received',
  templateUrl: './subscription-view-posts-received.page.html'
})
export class SubscriptionViewPostsReceivedPage {
  receivedPostsResponse: PaginatedResponse<PostSendInfo>;
  receivedPosts: PostSendInfo[];

  errorCode: number;

  constructor(route: ActivatedRoute, router: Router) {
    route.data.subscribe(data => {
      const receivedResponse: Resolved<PaginatedResponse<PostSendInfo>> = data.receivedResponse;

      if (receivedResponse.ok) {
        this.receivedPostsResponse = receivedResponse.content!;
        this.receivedPosts = this.receivedPostsResponse.items;
      } else {
        if (receivedResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }

        this.errorCode = receivedResponse.status;
        return;
      }
    });
  }
}
