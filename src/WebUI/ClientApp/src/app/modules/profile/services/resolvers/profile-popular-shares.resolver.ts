import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../../services/weshareclient";
import { Resolved } from "../../../../../types/general-types";
import { ShareSnippet } from "../../../../../types/share-types";


@Injectable()
export class ProfilePopularSharesResolver implements Resolve<Resolved<ShareSnippet[]>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Resolved<ShareSnippet[]>> {
    var username = route.parent!.params['username'];
    return this.weShareClient.getPopularShareSnippets(username, 0, 4)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<ShareSnippet[]>(error))),
      );
  }
}
