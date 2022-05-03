import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { AuthService } from "../../../../../services/authservice";
import { WeShareClient } from "../../../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../../../types/general-types";
import { SubscriptionSnippet } from "../../../../../types/subscription-types";

@Injectable()
export class ProfileSubscriptionSnippetsResolver implements Resolve<Resolved<PaginatedResponse<SubscriptionSnippet>>> {
  constructor(private weShareClient: WeShareClient, private authService: AuthService) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<SubscriptionSnippet>> | Observable<Resolved<PaginatedResponse<SubscriptionSnippet>>> | Promise < Resolved < PaginatedResponse < SubscriptionSnippet >>> {
    if (this.authService.isLoggedOut()) {
      return Resolved.errorCode<PaginatedResponse<SubscriptionSnippet>>(403);
    }

    const userId = this.authService.getUserId()!;
    return this.weShareClient.getSubscriptionSnippets(userId, null, 0, 10)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<SubscriptionSnippet>>(error))),
      );
  }
}
