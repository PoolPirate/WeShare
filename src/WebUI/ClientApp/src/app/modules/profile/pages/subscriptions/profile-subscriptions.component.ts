import { HttpErrorResponse, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';
import { WeShareClient } from '../../../../../services/weshareclient';
import { PaginatedResponse, Resolved } from '../../../../../types/general-types';
import { ShareSnippet } from '../../../../../types/share-types';
import { SubscriptionSnippet } from '../../../../../types/subscription-types';
import { ProfileStore } from '../../services/profile-store';

@Component({
  selector: 'profile-subscriptions',
  templateUrl: './profile-subscriptions.component.html',
  styleUrls: ['./profile-subscriptions.component.css']
})
export class ProfileSubscriptionsComponent {
  errorCode: number = 0;

  subscriptionSnippetsResponse: PaginatedResponse<SubscriptionSnippet>;
  subscriptionSnippets: SubscriptionSnippet[];

  constructor(private profileStore: ProfileStore, private authService: AuthService, private weShareClient: WeShareClient,
    route: ActivatedRoute, router: Router) {
    route.data.subscribe(data => {
      const subscriptionSnippetsResponse: Resolved<PaginatedResponse<SubscriptionSnippet>> = data.subscriptionSnippetsResponse;

      if (subscriptionSnippetsResponse.ok) {
        this.subscriptionSnippetsResponse = subscriptionSnippetsResponse.content!;
        this.subscriptionSnippets = this.subscriptionSnippetsResponse.items;
      } else {
        if (subscriptionSnippetsResponse.status == 404) {
          router.navigateByUrl("/notfound");
          return;
        }
        if (subscriptionSnippetsResponse.status == 403) {
          router.navigate(['profile', this.profileStore.userSnippet.username]);
          return;
        }
        this.errorCode = subscriptionSnippetsResponse.status;
        return;
      }
    });
  }

  refreshList(pageEvent: PageEvent) {
    this.weShareClient.getSubscriptionSnippets(this.userSnippet.id, null, pageEvent.pageIndex, pageEvent.pageSize)
      .subscribe(success => {
        this.subscriptionSnippetsResponse = success;
        this.subscriptionSnippets = this.subscriptionSnippetsResponse.items;
      }, error => {
        alert("There was an error while loading the data!");
      });
  }

  get userSnippet() {
    return this.profileStore.userSnippet;
  }

  get isOwnProfile() {
    return this.userSnippet.id == this.authService.getUserId();
  }
}
