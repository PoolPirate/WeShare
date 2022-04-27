import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResponse, Resolved } from '../../../../types/general-types';
import { PostMetadata } from '../../../../types/post-types';
import { ShareData } from '../../../../types/share-types';
import { ShareService } from '../../services/shareservice';

@Component({
  selector: 'share-view-posts',
  templateUrl: './share-view-posts.component.html',
  styleUrls: ['./share-view-posts.component.css']
})
export class ShareViewPostsComponent {
  shareData: ShareData;

  postsData: PaginatedResponse<PostMetadata>;
  posts: PostMetadata[];
  errorCode: number;

  constructor(shareService: ShareService, router: Router, route: ActivatedRoute) {
    this.shareData = shareService.shareData;

    route.data.subscribe(data => {
      var postsResponse: Resolved<PaginatedResponse<PostMetadata>> = data.postsResponse;

      if (postsResponse.ok) {
        this.postsData = postsResponse.content!;
        this.posts = this.postsData.items;
        shareService.shareData = this.shareData;
      } else {
        if (postsResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }

        this.errorCode = postsResponse.status;
        return;
      }
    });
  }
}
