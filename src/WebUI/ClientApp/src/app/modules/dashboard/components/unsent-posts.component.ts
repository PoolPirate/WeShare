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
  subscriptionPostMap: Compound[] = [];

  constructor(dashboardService: DashboardService, weShareClient: WeShareClient) {
    dashboardService.subscriptionInfos.forEach(subscriptionInfo => {
      weShareClient.getUnsentPosts(subscriptionInfo.id)
        .subscribe(response => {
          const compound = new Compound(subscriptionInfo, response.items);
          this.subscriptionPostMap.push(compound);
        });
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
