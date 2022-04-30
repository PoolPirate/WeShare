import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PostViewService } from '../../services/postviewservice';

@Component({
  selector: 'post-view-headers',
  templateUrl: './post-view-headers.page.html',
  styleUrls: ['./post-view-headers.page.css']
})
export class PostViewHeadersPage {
  entries: [string, string[]][] | null = null;

  constructor(private postViewService: PostViewService, router: Router) {
    if (this.postViewService.content == null) {
      router.navigate(["post", postViewService.postSnippet.id, "overview"]);
      return;
    }
  }

  get headers() {
    if (this.entries != null) {
      return this.entries;
    }

    this.entries = Array.from(this.postViewService.content.headers);
    return this.entries;
  }
}
