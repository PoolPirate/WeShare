import { Component } from "@angular/core";
import { SubscriptionViewService } from "../../services/subscriptionviewservice";

@Component({
  selector: 'subscription-view-posts',
  templateUrl: './subscription-view-posts.component.html',
  styleUrls: ['./subscription-view-posts.component.css']
})
export class SubscriptionViewPostsComponent {
  constructor(private subscriptionViewService: SubscriptionViewService) { }

  get subscriptionInfo() {
    return this.subscriptionViewService.subscriptionInfo;
  }
}
