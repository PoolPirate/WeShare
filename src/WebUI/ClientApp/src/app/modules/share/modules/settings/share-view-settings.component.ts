import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';
import { Resolved } from '../../../../../types/general-types';
import { ShareSecrets } from '../../../../../types/share-types';
import { ShareSecretsService } from './services/sharesecretsservice';

@Component({
  selector: 'share-view-settings',
  templateUrl: './share-view-settings.component.html',
  providers: [
    ShareSecretsService
  ]
})
export class ShareViewSettingsComponent {
  errorCode: number = 0;
  shareSecrets: ShareSecrets;

  constructor(route: ActivatedRoute, router: Router,
    shareSecretsService: ShareSecretsService, authService: AuthService)
  {
    route.url.subscribe(url => {
      if (authService.isLoggedOut()) {
        router.navigate(["forbidden"]);
      }
    });

    route.data.subscribe(data => {
      var secretsResponse: Resolved<ShareSecrets> = data.secretsResponse;

      if (secretsResponse.ok) {
        this.shareSecrets = secretsResponse.content!;
        shareSecretsService.shareSecrets = this.shareSecrets;
      } else {
        if (secretsResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }

        this.errorCode = secretsResponse.status;
        return;
      }
    });
  }
}
