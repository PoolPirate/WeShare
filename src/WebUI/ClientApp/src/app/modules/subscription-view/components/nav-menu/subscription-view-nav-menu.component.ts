import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';

@Component({
  selector: 'subscription-view-nav-menu',
  templateUrl: './subscription-view-nav-menu.component.html',
  styleUrls: ['./subscription-view-nav-menu.component.css']
})
export class SubscriptionViewNavMenuComponent {
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
