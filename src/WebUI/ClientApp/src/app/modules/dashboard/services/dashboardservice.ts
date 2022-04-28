import { Injectable } from "@angular/core";
import { PaginatedResponse } from "../../../../types/general-types";
import { SubscriptionSnippet } from "../../../../types/subscription-types";

@Injectable()
export class DashboardService {
  subscriptionInfos: SubscriptionSnippet[];
}
