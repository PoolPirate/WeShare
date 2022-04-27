import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'user-settings-nav-menu',
  templateUrl: './user-settings-nav-menu.component.html',
  styleUrls: ['./user-settings-nav-menu.component.css']
})
export class UserSettingsNavMenuComponent {
  activeLink: string;

  constructor(route: ActivatedRoute, router: Router) {
    route.url.subscribe(url => {
      this.activeLink = router.url.split('/').pop()!;
    });
  }
}
