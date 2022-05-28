import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { async } from 'rxjs';
import { AuthService } from '../../../../../../../services/authservice';
import { DialogService } from '../../../../../../../services/dialogservice';
import { WeShareClient } from '../../../../../../../services/weshareclient';
import { ShareData, ShareSecrets } from '../../../../../../../types/share-types';
import { ShareService } from '../../../../services/shareservice';
import { ShareSecretsService } from '../../services/sharesecretsservice';

@Component({
  selector: 'share-view-settings-critical',
  templateUrl: './share-view-settings-critical.component.html',
  styleUrls: ['./share-view-settings-critical.component.scss']
})
export class ShareViewSettingsCriticalComponent {
  shareSecrets: ShareSecrets;
  shareData: ShareData;

  deleteRan: boolean = false;
  visibilityChangeRan: boolean = false;

  errorCode: number = 0;

  constructor(private weShareClient: WeShareClient, private router: Router, private dialogService: DialogService, private authService: AuthService,
    shareSecretsService: ShareSecretsService, shareService: ShareService) {
    this.shareSecrets = shareSecretsService.shareSecrets;
    this.shareData = shareService.shareData;
  }
  
  async delete() {
    if (this.deleteRan) {
      return;
    }
    if (!await this.dialogService.confirm("Delete Share?", "All posts and subscribers will be permanently deleted")) {
      return;
    }
    
    this.weShareClient.deleteShare(this.shareData.shareInfo.id)
      .subscribe(response => {
        this.errorCode = 200;
        this.router.navigate(['profile', this.shareData.ownerSnippet.username, 'shares']);
      }, async (error: HttpErrorResponse) => {
        if (error.status == 404) {
          this.router.navigate(['profile', this.shareData.ownerSnippet.username, 'shares']);
          return;
        }
        if (error.status == 401) {
          if (!await this.authService.requestLogin()) {
            this.router.navigate(['/']);
            return;
          }

          return;
        }
        if (error.status == 403) {
          this.router.navigate(['forbidden']);
          return;
        }
        this.errorCode = error.status;
        this.deleteRan = false;
      });
  }

  async updateVisibility() {
    if (this.visibilityChangeRan) {
      return;
    }
    if (!await this.dialogService.confirm("Change Share Visibility?", "Private Shares cant have subs & likes")) {
      return;
    }

    this.weShareClient.updateShareVisibility(this.shareData.shareInfo.id, !this.shareData.shareInfo.isPrivate)
      .subscribe(response => {
        this.errorCode = 200;
      }, async (error: HttpErrorResponse) => {
        if (error.status == 404) {
          this.router.navigate(['profile', this.shareData.ownerSnippet.username, 'shares']);
          return;
        }
        if (error.status == 401) {
          if (!await this.authService.requestLogin()) {
            this.router.navigate(['/']);
            return;
          }

          return;
        }
        if (error.status == 403) {
          this.router.navigate(['forbidden']);
          return;
        }
        this.errorCode = error.status;
        this.visibilityChangeRan = false;
      });
  }
}
