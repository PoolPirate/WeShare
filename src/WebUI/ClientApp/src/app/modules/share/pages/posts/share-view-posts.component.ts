import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { WeShareClient } from '../../../../../services/weshareclient';
import { PaginatedResponse, Resolved } from '../../../../../types/general-types';
import { PostSnippet } from '../../../../../types/post-types';
import { ShareData } from '../../../../../types/share-types';
import { ShareService } from '../../services/shareservice';

@Component({
  selector: 'share-view-posts',
  templateUrl: './share-view-posts.component.html',
  styleUrls: ['./share-view-posts.component.css']
})
export class ShareViewPostsComponent {
  shareData: ShareData;

  postsResponse: PaginatedResponse<PostSnippet>;
  posts: PostSnippet[];
  errorCode: number;

  constructor(private weShareClient: WeShareClient, shareService: ShareService, router: Router, route: ActivatedRoute) {
    this.shareData = shareService.shareData;

    route.data.subscribe(data => {
      var postsResponse: Resolved<PaginatedResponse<PostSnippet>> = data.postsResponse;

      if (postsResponse.ok) {
        this.postsResponse = postsResponse.content!;
        this.posts = this.postsResponse.items;
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

  refreshList(pageEvent: PageEvent) {
    this.weShareClient.getPosts(this.shareData.shareInfo.id, pageEvent.pageIndex, pageEvent.pageSize)
      .subscribe(success => {
        this.postsResponse = success;
        this.posts = this.postsResponse.items;
      }, error => {
        alert("There was an error while loading the data!");
      });
  }
}
