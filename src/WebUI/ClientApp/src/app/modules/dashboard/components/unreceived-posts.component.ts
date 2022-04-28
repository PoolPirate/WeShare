import { Component } from '@angular/core';
import { WeShareClient } from '../../../../services/weshareclient';
import { PostSnippet } from '../../../../types/post-types';
import { SubscriptionSnippet } from '../../../../types/subscription-types';
import { DashboardService } from '../services/dashboardservice';

@Component({
  selector: 'unreceived-posts-menu',
  templateUrl: './unreceived-posts.component.html',
  styleUrls: ['./unreceived-posts.component.css']
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

  markAsRead(compound: Compound, postId: number) {
    this.weShareClient.markPostAsSent(compound.subscriptionSnippet.id, postId)
      .subscribe(
        success => {
          const index = compound.unsentPosts.findIndex(x => x.id == postId);
          compound.unsentPosts.splice(index, 1);
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
