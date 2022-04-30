import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PostViewService } from '../../services/postviewservice';

@Component({
  selector: 'post-view-overview',
  templateUrl: './post-view-overview.page.html',
  styleUrls: ['./post-view-overview.page.css']
})
export class PostViewOverviewPage {
  constructor(private postViewService: PostViewService) {
  }

  get shareSnippet() {
    return this.postViewService.shareSnippet;
  }

  get postSnippet() {
    return this.postViewService.postSnippet;
  }
}
