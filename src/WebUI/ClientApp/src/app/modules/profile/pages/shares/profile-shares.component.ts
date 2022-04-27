import { HttpResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';
import { PaginatedResponse, Resolved } from '../../../../types/general-types';
import { ShareSnippet } from '../../../../types/share-types';
import { UserSnippet } from '../../../../types/user-types';
import { ProfileStore } from '../../services/profile-store';

@Component({
  selector: 'profile-shares',
  templateUrl: './profile-shares.component.html',
  styleUrls: ['./profile-shares.component.css']
})
export class ProfileSharesComponent {
  shareSnippetsData: PaginatedResponse<ShareSnippet>;
  shareSnippets: ShareSnippet[];

  errorCode: number = 0;

  constructor(private profileStore: ProfileStore, private authService: AuthService,
    route: ActivatedRoute, router: Router) {
    route.data.subscribe(data => {
      var sharesResponse: Resolved<PaginatedResponse<ShareSnippet>> = data.sharesResponse;
      if (sharesResponse.ok) {
        this.shareSnippetsData = sharesResponse.content!;
        this.shareSnippets = this.shareSnippetsData.items;
      } else {
        if (sharesResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }
        this.errorCode = sharesResponse.status;
        return;
      }
    });
  }

  get userSnippet() {
    return this.profileStore.userSnippet;
  }
  get isOwnProfile() {
    return this.userSnippet.id == this.authService.getUserId();
  }
}
