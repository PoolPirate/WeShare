import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../../../../../types/general-types";
import { PostSnippet } from "../../../../../../../types/post-types";

@Injectable()
export class SubscriptionViewPostsUnsentResolver implements Resolve<Resolved<PaginatedResponse<PostSnippet>>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<PostSnippet>> | Observable<Resolved<PaginatedResponse<PostSnippet>>> | Promise<Resolved<PaginatedResponse<PostSnippet>>> {
    var subscriptionId = route.parent!.parent!.params['subscriptionId'];

    return this.weShareClient.getUnsentPosts(subscriptionId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<PostSnippet>>(error))),
      );
  }
}
