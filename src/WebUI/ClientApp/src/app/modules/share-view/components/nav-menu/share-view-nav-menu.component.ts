import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';
import { ShareService } from '../../services/shareservice';

@Component({
  selector: 'share-view-nav-menu',
  templateUrl: './share-view-nav-menu.component.html',
  styleUrls: ['./share-view-nav-menu.component.css']
})
export class ShareViewNavMenuComponent {
  isOwnProfile: boolean;
  activeLink: string;

  constructor(route: ActivatedRoute, router: Router, authService: AuthService, shareService: ShareService) {
    this.activeLink = router.url.split('/')[4]!;
    if (authService.isLoggedOut()) {
      return;
    }

    this.isOwnProfile = shareService.shareData.ownerSnippet.id == authService.getUserId();
  }
}
