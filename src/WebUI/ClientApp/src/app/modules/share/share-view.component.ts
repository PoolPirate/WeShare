import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/authservice';
import { Resolved } from '../../../types/general-types';
import { ShareData, ShareUserData } from '../../../types/share-types';
import { ShareService } from './services/shareservice';

@Component({
  selector: 'share-view',
  templateUrl: './share-view.component.html',
  providers: [
    ShareService
  ]
})
export class ShareViewComponent {
  shareId: number = NaN;
  tab: string = "";

  errorCode: number = 0;
  shareData: ShareData;
  shareUserData: ShareUserData;

  constructor(route: ActivatedRoute, router: Router, shareService: ShareService, authService: AuthService) {
    route.data.subscribe(data => {
      var shareInfoResponse: Resolved<ShareData> = data.shareInfoResponse;

      if (shareInfoResponse.ok) {
        this.shareData = shareInfoResponse.content!;
        shareService.shareData = this.shareData;
      } else {
        if (shareInfoResponse.status == 404) {
          router.navigate(["notfound"]);
          return;
        }
        if (shareInfoResponse.status == 403) {
          router.navigate(["forbidden"]);
          return;
        }

        this.errorCode = shareInfoResponse.status;
        return;
      }

      if (authService.isLoggedOut()) {
        return;
      }

      var shareUserDataResponse: Resolved<ShareUserData> = data.shareUserDataResponse;

      if (shareUserDataResponse.ok) {
        this.shareUserData = shareUserDataResponse.content!;
        shareService.shareUserData = this.shareUserData;
      } else {
        if (shareInfoResponse.status == 404) {
          router.navigate(["notfound"]);
          return;
        }
        if (shareInfoResponse.status == 403) {
          router.navigate(["forbidden"]);
          return;
        }

        this.errorCode = shareInfoResponse.status;
        return;
      }
    });
  }
}
