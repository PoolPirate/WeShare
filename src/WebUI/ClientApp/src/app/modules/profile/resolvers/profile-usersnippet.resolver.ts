import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../services/weshareclient";
import { Resolved } from "../../../../types/general-types";
import { UserSnippet } from "../../../../types/user-types";

@Injectable()
export class ProfileUserSnippetResolver implements Resolve<Resolved<UserSnippet>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<UserSnippet> | Observable<Resolved<UserSnippet>> | Promise<Resolved<UserSnippet>> {
    var username = route.params['username'];
    return this.weShareClient.getUserSnippet(username)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<UserSnippet>(error))),
      );
  }
}
