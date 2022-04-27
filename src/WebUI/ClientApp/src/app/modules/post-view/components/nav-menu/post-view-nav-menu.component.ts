import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';

@Component({
  selector: 'post-view-nav-menu',
  templateUrl: './post-view-nav-menu.component.html',
  styleUrls: ['./post-view-nav-menu.component.css']
})
export class PostViewNavMenuComponent {
  activeLink: string;

  constructor(route: ActivatedRoute, router: Router, authService: AuthService) {
    if (authService.isLoggedOut()) {
      return;
    }

    route.url.subscribe(url => {
      this.activeLink = router.url.split('/').pop()!.toLowerCase();
    });
  }
}
