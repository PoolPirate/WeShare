import { HttpErrorResponse, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';
import { PaginatedResponse, Resolved } from '../../../../types/general-types';
import { ShareInfo, ShareSnippet } from '../../../../types/share-types';
import { ProfileStore } from '../../services/profile-store';

@Component({
  selector: 'profile-likes',
  templateUrl: './profile-likes.component.html',
  styleUrls: ['./profile-likes.component.css']
})
export class ProfileLikesComponent {
  errorCode: number = 0;

  likedShareData: PaginatedResponse<ShareSnippet>;
  likedShareSnippets: ShareSnippet[];

  constructor(private profileStore: ProfileStore, private authService: AuthService,
    route: ActivatedRoute, router: Router) {
    route.data.subscribe(data => {
      var likesResponse: Resolved<PaginatedResponse<ShareSnippet>> = data.likesResponse;

      if (likesResponse.ok) {
        this.likedShareData = likesResponse.content!;
        this.likedShareSnippets = this.likedShareData.items;
      } else {
        if (likesResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }
        if (likesResponse.status == 403) {
          router.navigate(['profile', this.profileStore.userSnippet.username]);
          return;
        }
        this.errorCode = likesResponse.status;
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
