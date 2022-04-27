import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'share-view-edit-nav-menu',
  templateUrl: './share-view-edit-nav-menu.component.html',
  styleUrls: ['./share-view-edit-nav-menu.component.css']
})
export class ShareViewEditNavMenuComponent {
  activeLink: string;

  constructor(router: Router) {
    this.activeLink = router.url.split('/').pop()!;
  }
}
