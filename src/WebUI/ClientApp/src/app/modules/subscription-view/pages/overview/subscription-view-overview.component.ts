import { Component } from "@angular/core";
import { SubscriptionViewService } from "../../services/subscriptionviewservice";

@Component({
  selector: 'subscription-view-overview',
  templateUrl: './subscription-view-overview.component.html',
  styleUrls: ['./subscription-view-overview.component.css']
})
export class SubscriptionViewOverviewComponent {
  constructor(private subscriptionViewService: SubscriptionViewService) { }

  get subscriptionInfo() {
    return this.subscriptionViewService.subscriptionInfo;
  }
}
