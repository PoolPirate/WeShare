import { Component } from '@angular/core';
import { ShareData, ShareInfo, ShareSecrets } from '../../../../types/share-types';
import { ShareService } from '../../../share-view/services/shareservice';
import { ShareSecretsService } from '../../services/sharesecretsservice';

@Component({
  selector: 'share-view-settings-details',
  templateUrl: './share-view-settings-details.component.html'
})
export class ShareViewSettingsDetailsComponent {
  shareSecrets: ShareSecrets;
  shareData: ShareData;

  constructor(shareSecretsService: ShareSecretsService, shareService: ShareService) {
    this.shareSecrets = shareSecretsService.shareSecrets;
    this.shareData = shareService.shareData;
  }
}
