import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/authservice';
import { Resolved } from '../../../types/general-types';
import { SubscriptionInfo } from '../../../types/subscription-types';
import { SubscriptionViewService } from './services/subscriptionviewservice';

@Component({
  selector: 'subscription-view',
  templateUrl: './subscription-view.component.html',
  providers: [SubscriptionViewService]
})
export class SubscriptionViewComponent {
  errorCode: number = 0;
  subscriptionInfo: SubscriptionInfo;

  constructor(route: ActivatedRoute, router: Router, authService: AuthService, subscriptionViewService: SubscriptionViewService) {
    route.data.subscribe(data => {
      var shareInfoResponse: Resolved<SubscriptionInfo> = data.subscriptionInfoResponse;

      if (shareInfoResponse.ok) {
        this.subscriptionInfo = shareInfoResponse.content!;
        subscriptionViewService.subscriptionInfo = this.subscriptionInfo;
      } else {
        if (shareInfoResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }
        if (shareInfoResponse.status == 403) {
          router.navigateByUrl("/forbidden");
          return;
        }

        this.errorCode = shareInfoResponse.status;
        return;
      }

      if (authService.isLoggedOut()) {
        return;
      }
    });
  }
}
