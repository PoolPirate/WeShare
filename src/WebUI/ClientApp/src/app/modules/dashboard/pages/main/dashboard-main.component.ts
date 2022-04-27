import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';
import { PaginatedResponse, Resolved } from '../../../../../types/general-types';
import { SubscriptionSnippet } from '../../../../../types/subscription-types';
import { DashboardService } from '../../services/dashboardservice';

@Component({
  selector: 'dashboard-main',
  templateUrl: './dashboard-main.component.html',
  styleUrls: ['./dashboard-main.component.css']
})
export class DashboardMainComponent {
  errorCode: number = 0;
  subscriptionInfosResponse: PaginatedResponse<SubscriptionSnippet>;
  subscriptionInfos: SubscriptionSnippet[];

  constructor(authService: AuthService, router: Router, route: ActivatedRoute, dashboardService: DashboardService) {
    if (authService.isLoggedOut()) {
      router.navigate(['login']);
    }

    route.data.subscribe(data => {
      const subscriptionInfosResponse: Resolved<PaginatedResponse<SubscriptionSnippet>> = data.subscriptionSnippetsResponse;

      if (subscriptionInfosResponse.ok) {
        this.subscriptionInfosResponse = subscriptionInfosResponse.content!;
        this.subscriptionInfos = this.subscriptionInfosResponse.items;
        dashboardService.subscriptionInfos = this.subscriptionInfos;
      } else {
        if (subscriptionInfosResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }
        if (subscriptionInfosResponse.status == 403) {
          router.navigateByUrl("/forbidden");
          return;
        }

        this.errorCode = subscriptionInfosResponse.status;
        return;
      }
    });
  }
}
