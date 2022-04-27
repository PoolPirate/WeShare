import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { AuthService } from "../../../services/authservice";
import { WeShareClient } from "../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../types/general-types";
import { PostMetadata } from "../../../types/post-types";
import { SubscriptionInfo } from "../../../types/subscription-types";

@Injectable()
export class DashboardSubscriptionInfosResolver implements Resolve<Resolved<PaginatedResponse<SubscriptionInfo>>> {
  constructor(private authService: AuthService, private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<SubscriptionInfo>> | Observable<Resolved<PaginatedResponse<SubscriptionInfo>>> | Promise<Resolved<PaginatedResponse<SubscriptionInfo>>> {
    const userId = this.authService.getUserId()!;
    return this.weShareClient.getSubscriptions(userId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<PostMetadata>>(error))),
      );
  }
}
