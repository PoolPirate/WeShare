import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
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

  errorCode: number = 0;

  constructor(private weShareClient: WeShareClient, private router: Router,
    shareSecretsService: ShareSecretsService, shareService: ShareService) {
    this.shareSecrets = shareSecretsService.shareSecrets;
    this.shareData = shareService.shareData;
  }

  delete() {
    if (this.deleteRan) {
      return;
    }
    if (confirm("Do you  want to delete this?")) {
      if (confirm("Do you really want to delete this?")) {
        if (confirm("Do you REAAAAAALLY want to delete this?")) {
          if (confirm("Are you 1000% sure you want to delete this?")) {
            this.deleteRan = true;
          }
          else {
            return;
          }
        }
        else {
          return;
        }
      }
      else {
        return;
      }
    }
    else {
      return;
    }
    
    this.weShareClient.deleteShare(this.shareData.shareInfo.id)
      .subscribe(response => {
        this.errorCode = 200;
        this.router.navigate(['profile', this.shareData.ownerSnippet.username, 'shares']);
      }, (error: HttpErrorResponse) => {
        if (error.status == 404) {
          this.router.navigate(['profile', this.shareData.ownerSnippet.username, 'shares']);
          return;
        }
        if (error.status == 403) {
          this.router.navigate(['login']);
          return;
        }
        this.errorCode = error.status;
        this.deleteRan = false;
      });
  }
}
