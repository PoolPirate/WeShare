import { Component } from '@angular/core';
import { WeShareClient } from '../../../services/weshareclient';
import { PostMetadata } from '../../../types/post-types';
import { SubscriptionInfo } from '../../../types/subscription-types';
import { DashboardService } from '../services/dashboardservice';

@Component({
  selector: 'unsent-posts-menu',
  templateUrl: './unsent-posts.component.html'
})
export class UnsentPostsComponent {
  subscriptionPostMap: Compound[] | null = null;

  constructor(private dashboardService: DashboardService, private weShareClient: WeShareClient) {
    dashboardService.subscriptionInfos.forEach(subscriptionInfo => {
      weShareClient.getUnsentPosts(subscriptionInfo.id)
        .subscribe(response => {
          const compound = new Compound(subscriptionInfo, response.items);

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
    this.weShareClient.markPostAsSent(compound.subscriptionInfo.id, compound.unsentPosts[0].id)
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
  subscriptionInfo: SubscriptionInfo;
  unsentPosts: PostMetadata[];

  constructor(subscriptionInfo: SubscriptionInfo, unsentPosts: PostMetadata[]) {
    this.subscriptionInfo = subscriptionInfo;
    this.unsentPosts = unsentPosts;
  }
}
