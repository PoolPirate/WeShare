import { Injectable } from "@angular/core";
import { SubscriptionInfo } from "../../../types/subscription-types";

@Injectable()
export class DashboardService {
  subscriptionInfos: SubscriptionInfo[];
}
