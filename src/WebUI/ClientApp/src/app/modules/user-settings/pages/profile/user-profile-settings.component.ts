import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';
import { WeShareClient } from '../../../../services/weshareclient';
import { Resolved } from '../../../../types/general-types';
import { ProfileInfo } from '../../../../types/profile-types';
import { ProfileStore } from '../../../profile/services/profile-store';

@Component({
  selector: 'user-profile-settings',
  templateUrl: './user-profile-settings.component.html',
  styleUrls: ['./user-profile-settings.component.css']
})
export class UserProfileSettingsComponent {
  nickname: FormControl;
  likesPublished: FormControl;

  setup() {
    this.nickname = new FormControl(this.profileInfo.nickname, [Validators.minLength(3), Validators.maxLength(20)]);
    this.likesPublished = new FormControl(this.profileInfo.likesPublished);
  }

  profileInfo: ProfileInfo;

  ranForNickname: string = "";
  ranForLikesPublished: boolean | null;
  errorCode: number = 0;

  constructor(private weShareClient: WeShareClient, private authService: AuthService, private router: Router,
              route: ActivatedRoute) {
    route.data.subscribe(data => {
      var profileInfoResponse: Resolved<ProfileInfo> = data.profileInfoResponse;

      if (profileInfoResponse.ok) {
        this.profileInfo = profileInfoResponse.content!;
        this.setup();
      } else {
        if (profileInfoResponse.status == 404) {
          router.navigateByUrl("/");
          return;
        }

        this.errorCode = profileInfoResponse.status;
        return;
      }
    });
  }

  isInvalid() {
    return this.nickname.invalid;
  }
  markAllAsTouched() {
    this.nickname.markAsTouched();
  }

  updateProfile() {
    const oldErrorCode = this.errorCode;
    this.errorCode = 0;

    const nickname = this.nickname.value;
    const likesPublished = this.likesPublished.value;

    if (this.authService.getNickname() == nickname && this.profileInfo.likesPublished == likesPublished) {
      this.errorCode = 200; return;
    }
    if (this.ranForNickname == nickname && this.ranForLikesPublished == likesPublished) {
      this.errorCode = oldErrorCode; return;
    }

    this.ranForNickname = nickname;
    this.ranForLikesPublished = likesPublished;

    this.weShareClient.updateProfile(nickname, likesPublished)
      .subscribe(response => {
        this.errorCode = 200;
        this.authService.setNickname(nickname);
        return;
      }, (error: HttpErrorResponse) => {
        if (error.status == 410) {
          this.router.navigateByUrl("/");
          return;
        }

        this.errorCode = error.status;
      });
  }
}
