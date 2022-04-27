import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';
import { PaginatedResponse, Resolved } from '../../../../types/general-types';
import { ShareSnippet } from '../../../../types/share-types';
import { ProfileStore } from '../../services/profile-store';

@Component({
  selector: 'profile-overview',
  templateUrl: './profile-overview.component.html',
  styleUrls: ['./profile-overview.component.css']
})
export class ProfileOverviewComponent {
  popularShareSnippetsResponse: PaginatedResponse<ShareSnippet>;
  popularShareSnippets: ShareSnippet[];

  errorCode: number = 0;

  constructor(private profileStore: ProfileStore, private authService: AuthService,
    route: ActivatedRoute, router: Router) {
    route.data.subscribe(data => {
      var popularSharesResponse: Resolved<PaginatedResponse<ShareSnippet>> = data.popularSharesResponse;

      if (popularSharesResponse.ok) {
        this.popularShareSnippetsResponse = popularSharesResponse.content!;
        this.popularShareSnippets = this.popularShareSnippetsResponse.items;
      } else {
        if (popularSharesResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }

        this.errorCode = popularSharesResponse.status;
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
