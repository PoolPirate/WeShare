import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResponse, Resolved } from '../../../../../../../types/general-types';
import { PostSnippet } from '../../../../../../../types/post-types';

@Component({
  selector: 'subscription-view-posts-unsent',
  templateUrl: './subscription-view-posts-unsent.page.html'
})
export class SubscriptionViewPostsUnsentPage {
  unsentPostsResponse: PaginatedResponse<PostSnippet>;
  unsentPosts: PostSnippet[];

  errorCode: number;

  constructor(route: ActivatedRoute, router: Router) {
    route.data.subscribe(data => {
      const unsentResponse: Resolved<PaginatedResponse<PostSnippet>> = data.unsentResponse;

      if (unsentResponse.ok) {
        this.unsentPostsResponse = unsentResponse.content!;
        this.unsentPosts = this.unsentPostsResponse.items;
      } else {
        if (unsentResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }

        this.errorCode = unsentResponse.status;
        return;
      }
    });
  }
}
