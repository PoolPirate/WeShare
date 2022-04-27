import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resolved } from '../../types/general-types';
import { ProfileInfo } from '../../types/profile-types';
import { UserSnippet } from '../../types/user-types';
import { ProfileStore } from './services/profile-store';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  providers: []
})
export class ProfileComponent {
  errorCode: number;
  profileInfo: ProfileInfo;
  userSnippet: UserSnippet;

  constructor(profileStore: ProfileStore, route: ActivatedRoute, router: Router) {
    route.data.subscribe(data => {
      var profileInfoResponse: Resolved<ProfileInfo> = data.profileInfoResponse;
      var userSnippetResponse: Resolved<UserSnippet> = data.userSnippetResponse;

      if (profileInfoResponse.ok) {
        this.profileInfo = profileInfoResponse.content!;
        profileStore.profileInfo = this.profileInfo;
      } else {
        if (profileInfoResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }

        this.errorCode = profileInfoResponse.status;
        return;
      }

      if (userSnippetResponse.ok) {
        this.userSnippet = userSnippetResponse.content!;
        profileStore.userSnippet = this.userSnippet;
        console.info(profileStore);
      } else {
        if (userSnippetResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }

        this.errorCode = userSnippetResponse.status;
        return;
      }
    });
  }
}
