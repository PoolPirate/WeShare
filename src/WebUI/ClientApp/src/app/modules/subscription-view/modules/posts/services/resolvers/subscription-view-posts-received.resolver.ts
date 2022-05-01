import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../../../../../types/general-types";
import { SentPostInfoDto } from "../../../../../../../types/post-types";

@Injectable()
export class SubscriptionViewPostsReceivedResolver implements Resolve<Resolved<PaginatedResponse<SentPostInfoDto>>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<SentPostInfoDto>> | Observable<Resolved<PaginatedResponse<SentPostInfoDto>>> | Promise<Resolved<PaginatedResponse<SentPostInfoDto>>> {
    var subscriptionId = route.parent!.parent!.params['subscriptionId'];

    return this.weShareClient.getReceivedPosts(subscriptionId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<SentPostInfoDto>>(error))),
      );
  }
}
