import { Injectable } from "@angular/core";
import { ActivatedRoute, ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../services/weshareclient";
import { Resolved } from "../../../../types/general-types";
import { SubscriptionInfo } from "../../../../types/subscription-types";

@Injectable()
export class SubscriptionViewSubscriptionInfoResolver implements Resolve<Resolved<SubscriptionInfo>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<SubscriptionInfo> | Observable<Resolved<SubscriptionInfo>> | Promise<Resolved<SubscriptionInfo>> {
    const subscriptionId = route.params["subscriptionId"];
    return this.weShareClient.getSubscriptionInfo(subscriptionId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<SubscriptionInfo>(error))),
      );
  }
}
