import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/authservice';
import { PostViewService } from './services/postviewservice';

@Component({
  selector: 'post-view',
  templateUrl: './post-view.component.html',
  providers: [PostViewService]
})
export class PostViewComponent {
  errorCode: number = 0;

  constructor(route: ActivatedRoute, router: Router, authService: AuthService) {
    //route.data.subscribe(data => {
    //  var shareInfoResponse: Resolved<SubscriptionInfo> = data.subscriptionInfoResponse;

    //  if (shareInfoResponse.ok) {
    //    this.subscriptionInfo = shareInfoResponse.content!;
    //    subscriptionViewService.subscriptionInfo = this.subscriptionInfo;
    //  } else {
    //    if (shareInfoResponse.status == 404) {
    //      router.navigateByUrl("/notfound");
    //      return;
    //    }
    //    if (shareInfoResponse.status == 403) {
    //      router.navigateByUrl("/forbidden");
    //      return;
    //    }

    //    this.errorCode = shareInfoResponse.status;
    //    return;
    //  }

    //  if (authService.isLoggedOut()) {
    //    return;
    //  }
    //});
  }
}
