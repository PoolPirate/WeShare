import { HttpErrorResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';
import { WeShareClient } from '../../../../services/weshareclient';
import { ShareData, ShareInfo, ShareUserData } from '../../../../types/share-types';
import { LikeButtonComponent } from '../../../shared/components/like-button/like-button.component';
import { ShareService } from '../../services/shareservice';

@Component({
  selector: 'share-view-overview',
  templateUrl: './share-view-overview.component.html',
  styleUrls: ['./share-view-overview.component.scss']
})
export class ShareViewOverviewComponent {
  shareData: ShareData;
  shareUserData: ShareUserData | null;

  @ViewChild(LikeButtonComponent) likebutton: LikeButtonComponent;

  constructor(private weShareClient: WeShareClient, private authService: AuthService, private router: Router,
    shareService: ShareService) {
    this.shareData = shareService.shareData;
    this.shareUserData = shareService.shareUserData;
  }

  like(liked: boolean) {
    if (this.authService.isLoggedOut()) {
      this.router.navigateByUrl("/login");
    }

    if (liked) {
      this.weShareClient.addLike(this.shareData.shareInfo.id).subscribe(response => {
        this.shareUserData!.liked = liked;
        this.shareData.shareInfo.likeCount++;
      }, (error: HttpErrorResponse) => {
        if (error.status == 409) {
          this.likebutton.liked = true;
          this.shareUserData!.liked = true;
        } else {
          this.likebutton.liked = !liked;
        }
      });
    } else {
      this.weShareClient.removeLike(this.shareData.shareInfo.id).subscribe(response => {
        this.shareUserData!.liked = liked;
        this.shareData.shareInfo.likeCount--;
      }, (error: HttpErrorResponse) => {
        if (error.status == 404) {
          this.likebutton.liked = false;
          this.shareUserData!.liked = false;
        } else {
          this.likebutton.liked = !liked;
        }
      });
    }   
  }
}
