import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PostViewService } from '../../services/postviewservice';

@Component({
  selector: 'post-view-payload',
  templateUrl: './post-view-payload.page.html',
  styleUrls: ['./post-view-payload.page.css']
})
export class PostViewPayloadPage {
  constructor(private postViewService: PostViewService, router: Router) {
    if (this.postViewService.content == null) {
      router.navigate(["post", postViewService.postSnippet.id, "overview"]);
      return;
    }
  }

  get payloadRaw() {
    return this.postViewService.content.payload;
  }

  get payloadText() { 
    return new TextDecoder().decode(this.payloadRaw);
  }

  get isText() {
    const typeA = this.postViewService.content.headers.get("Content-Type");

    if (typeA == undefined) {
      return false;
    }

    const type = typeA[0];
    return type.startsWith("application/json") ||
           type.startsWith("text/plain");
  }
}
