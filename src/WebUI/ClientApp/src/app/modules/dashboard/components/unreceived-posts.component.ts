import { Component } from '@angular/core';
import { WeShareClient } from '../../../../services/weshareclient';
import { PostSnippet } from '../../../../types/post-types';
import { SubscriptionSnippet } from '../../../../types/subscription-types';
import { DashboardService } from '../services/dashboardservice';

@Component({
  selector: 'unreceived-posts-menu',
  templateUrl: './unreceived-posts.component.html'
})
export class UnreceivedPostsComponent {
  subscriptionPostMap: Compound[] | null = null;

  constructor(private dashboardService: DashboardService, private weShareClient: WeShareClient) {
    dashboardService.subscriptionInfos.forEach(subscriptionSnippet => {
      weShareClient.getUnsentPosts(subscriptionSnippet.id)
        .subscribe(response => {
          const compound = new Compound(subscriptionSnippet, response.items);

          if (!this.subscriptionPostMap) {
            this.subscriptionPostMap = [];
          }
          if (response.items.length == 0) {
            return;
          }

          this.subscriptionPostMap.push(compound);
        });
    });
  }

  markAsRead(compound: Compound) {
    this.weShareClient.markPostAsSent(compound.subscriptionSnippet.id, compound.unsentPosts[0].id)
      .subscribe(
        success => {
          compound.unsentPosts.splice(0, 1);
        },
        error => {
          alert(error.status);
        });
  }
}

class Compound {
  subscriptionSnippet: SubscriptionSnippet;
  unsentPosts: PostSnippet[];

  constructor(subscriptionSnippet: SubscriptionSnippet, unsentPosts: PostSnippet[]) {
    this.subscriptionSnippet = subscriptionSnippet;
    this.unsentPosts = unsentPosts;
  }
}
