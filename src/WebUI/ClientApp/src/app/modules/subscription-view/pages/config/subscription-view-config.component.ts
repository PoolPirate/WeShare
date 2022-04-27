import { Component } from "@angular/core";
import { SubscriptionViewService } from "../../services/subscriptionviewservice";

@Component({
  selector: 'subscription-view-config',
  templateUrl: './subscription-view-config.component.html',
  styleUrls: ['./subscription-view-config.component.css']
})
export class SubscriptionViewConfigComponent {
  constructor(private subscriptionViewService: SubscriptionViewService) { }

  get subscriptionInfo() {
    return this.subscriptionViewService.subscriptionInfo;
  }
}
