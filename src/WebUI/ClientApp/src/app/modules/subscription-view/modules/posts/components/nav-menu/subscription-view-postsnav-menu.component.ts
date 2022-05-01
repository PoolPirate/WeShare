import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'subscription-view-posts-nav-menu',
  templateUrl: './subscription-view-posts-nav-menu.component.html',
  styleUrls: ['./subscription-view-posts-nav-menu.component.css']
})
export class SubscriptionViewPostsNavMenuComponent {
  activeLink: string;

  constructor(router: Router, route: ActivatedRoute) {
    route.url.subscribe(url => {
      this.activeLink = router.url.split('/').pop()!.toLowerCase();
    });
  }
}
