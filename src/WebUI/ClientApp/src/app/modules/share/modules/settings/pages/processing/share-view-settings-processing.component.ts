import { Component } from '@angular/core';
import { ShareData, ShareSecrets } from '../../../../../../../types/share-types';
import { ShareService } from '../../../../services/shareservice';
import { ShareSecretsService } from '../../services/sharesecretsservice';

@Component({
  selector: 'share-view-settings-processing',
  templateUrl: './share-view-settings-processing.component.html'
})
export class ShareViewSettingsProcessingComponent {
  shareSecrets: ShareSecrets;
  shareData: ShareData;

  constructor(shareSecretsService: ShareSecretsService, shareService: ShareService) {
    this.shareSecrets = shareSecretsService.shareSecrets;
    this.shareData = shareService.shareData;
  }
}
