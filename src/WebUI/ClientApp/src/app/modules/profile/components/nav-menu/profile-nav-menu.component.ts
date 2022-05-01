import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';
import { ProfileStore } from '../../services/profile-store';

@Component({
  selector: 'profile-nav-menu',
  templateUrl: './profile-nav-menu.component.html',
  styleUrls: ['./profile-nav-menu.component.css']
})
export class ProfileNavMenuComponent {
  activeLink: string;

  constructor(private profileStore: ProfileStore, private authService: AuthService, route: ActivatedRoute, router: Router) {
    route.url.subscribe(url => {
      this.activeLink = router.url.split('/').pop()!;
    });
  }

  get isOwnProfile() {
    return this.profileStore.userSnippet.id == this.authService.getUserId();
  }
  get showLikesTab() {
    return this.isOwnProfile || this.profileStore.profileInfo.likesPublished;
  }
  get showSubscriptionsTab() {
    return this.isOwnProfile;
  }
}
