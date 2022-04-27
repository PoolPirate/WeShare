import { Injectable } from "@angular/core";
import { SubscriptionSnippet } from "../../../../types/subscription-types";

@Injectable()
export class DashboardService {
  subscriptionInfos: SubscriptionSnippet[];
}
