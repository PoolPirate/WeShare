import { Component } from '@angular/core';
import { ActivatedRoute, Resolve, Router } from '@angular/router';
import { AuthService } from '../../../services/authservice';
import { Resolved } from '../../../types/general-types';
import { PostContent, PostSnippet } from '../../../types/post-types';
import { ShareSnippet } from '../../../types/share-types';
import { PostViewService } from './services/postviewservice';

@Component({
  selector: 'post-view',
  templateUrl: './post-view.component.html',
  providers: [PostViewService]
})
export class PostViewComponent {
  constructor(route: ActivatedRoute, router: Router, authService: AuthService, postViewService: PostViewService) {
    route.data.subscribe(data => {
      const snippetsResponse: Resolved<[ShareSnippet, PostSnippet]> = data.snippetsResponse;

      if (snippetsResponse.ok) {
        postViewService.shareSnippet = snippetsResponse.content![0];
          postViewService.postSnippet = snippetsResponse.content![1];
      } else {
        if (snippetsResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }
        if (snippetsResponse.status == 403) {
          router.navigateByUrl("/forbidden");
          return;
        }
        return;
      }

      const postContentResponse: Resolved<PostContent> = data.postContentResponse;

      if (postContentResponse.ok) {
        postViewService.content = postContentResponse.content!;
      } else {
        if (postContentResponse.status == 404) {
          return; //Post exists but content already deleted
        }
        if (postContentResponse.status == 403) {
          router.navigateByUrl("/forbidden");
          return;
        }
        return;
      }

    });
  }
}
