import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../../types/general-types";
import { ShareSnippet } from "../../../../types/share-types";

@Injectable()
export class ProfileLikesResolver implements Resolve<Resolved<PaginatedResponse<ShareSnippet>>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Resolved<PaginatedResponse<ShareSnippet>>> {
    var username = route.parent!.params['username'];
    return this.weShareClient.getLikedShareSnippets(username, 0, 10)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<ShareSnippet>>(error))),
      );
  }
}
