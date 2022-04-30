import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';
import { PostViewService } from '../../services/postviewservice';

@Component({
  selector: 'post-view-nav-menu',
  templateUrl: './post-view-nav-menu.component.html',
  styleUrls: ['./post-view-nav-menu.component.css']
})
export class PostViewNavMenuComponent {
  activeLink: string;

  constructor(private postViewService: PostViewService, route: ActivatedRoute, router: Router) {
    route.url.subscribe(url => {
      this.activeLink = router.url.split('/').pop()!.toLowerCase();
    });
  }

  get hasContent() {
    return this.postViewService.content != null;
  }
}
