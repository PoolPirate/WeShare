import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../../../../../types/general-types";
import { PostSendInfo } from "../../../../../../../types/post-types";

@Injectable()
export class SubscriptionViewPostsPendingResolver implements Resolve<Resolved<PaginatedResponse<PostSendInfo>>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<PostSendInfo>> | Observable<Resolved<PaginatedResponse<PostSendInfo>>> | Promise<Resolved<PaginatedResponse<PostSendInfo>>> {
    var subscriptionId = route.parent!.parent!.params['subscriptionId'];

    return this.weShareClient.getPendingPosts(subscriptionId, 0, 10)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<PostSendInfo>>(error))),
      );
  }
}
