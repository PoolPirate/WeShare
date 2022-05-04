import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../services/weshareclient";
import { ServiceConnectionSnippet } from "../../../../types/account-types";
import { PaginatedResponse, Resolved } from "../../../../types/general-types";

@Injectable()
export class UserSettingsServiceConnectionsResolver implements Resolve<Resolved<PaginatedResponse<ServiceConnectionSnippet>>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<ServiceConnectionSnippet>> | Observable<Resolved<PaginatedResponse<ServiceConnectionSnippet>>> | Promise<Resolved<PaginatedResponse<ServiceConnectionSnippet>>> {
    return this.weShareClient.getServiceConnections(0, 10)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<ServiceConnectionSnippet>>(error))),
      );
  }
}
