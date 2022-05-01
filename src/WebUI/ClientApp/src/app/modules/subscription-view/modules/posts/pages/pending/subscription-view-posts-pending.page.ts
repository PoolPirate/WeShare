import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResponse, Resolved } from '../../../../../../../types/general-types';
import { SentPostInfoDto } from '../../../../../../../types/post-types';

@Component({
  selector: 'subscription-view-posts-pending',
  templateUrl: './subscription-view-posts-pending.page.html'
})
export class SubscriptionViewPostsPendingPage {
  pendingPostsResponse: PaginatedResponse<SentPostInfoDto>;
  pendingPosts: SentPostInfoDto[];

  errorCode: number;

  constructor(route: ActivatedRoute, router: Router) {
    route.data.subscribe(data => {
      const pendingResponse: Resolved<PaginatedResponse<SentPostInfoDto>> = data.pendingResponse;

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
}
