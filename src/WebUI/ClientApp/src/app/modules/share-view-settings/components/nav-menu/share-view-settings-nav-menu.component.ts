import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'share-view-settings-nav-menu',
  templateUrl: './share-view-settings-nav-menu.component.html',
  styleUrls: ['./share-view-settings-nav-menu.component.css']
})
export class ShareViewSettingsNavMenuComponent {
  activeLink: string;

  constructor(router: Router) {
    this.activeLink = router.url.split('/').pop()!;
  }
}
