import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { AuthService } from "../../../../../services/authservice";
import { WeShareClient } from "../../../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../../../types/general-types";
import { SubscriptionSnippet } from "../../../../../types/subscription-types";


@Injectable()
export class ShareViewSubscriptionSnippetsResolver implements Resolve<Resolved<PaginatedResponse<SubscriptionSnippet>>> {
  constructor(private authService: AuthService, private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<SubscriptionSnippet>> | Observable<Resolved<PaginatedResponse<SubscriptionSnippet>>> | Promise<Resolved<PaginatedResponse<SubscriptionSnippet>>> {
    const userId = this.authService.getUserId()!;
    const shareId = route.parent?.params["shareId"];
    return this.weShareClient.getShareSubscriptionSnippets(userId, shareId, 0, 10)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<SubscriptionSnippet>>(error))),
      );
  }
}
