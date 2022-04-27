import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { AuthService } from "../../../../services/authservice";
import { WeShareClient } from "../../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../../types/general-types";
import { SubscriptionSnippet } from "../../../../types/subscription-types";

@Injectable()
export class SubscriptionViewSubscriptionSnippetsResolver implements Resolve<Resolved<PaginatedResponse<SubscriptionSnippet>>> {
  constructor(private authService: AuthService, private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<SubscriptionSnippet>> | Observable<Resolved<PaginatedResponse<SubscriptionSnippet>>> | Promise<Resolved<PaginatedResponse<SubscriptionSnippet>>> {
    const userId = this.authService.getUserId()!;
    return this.weShareClient.getSubscriptionSnippets(userId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<SubscriptionSnippet>>(error))),
      );
  }
}
