import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../services/weshareclient";
import { Resolved } from "../../../../types/general-types";
import { ProfileInfo } from "../../../../types/profile-types";

@Injectable()
export class ProfileProfileInfoResolver implements Resolve<Resolved<ProfileInfo>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<ProfileInfo> | Observable<Resolved<ProfileInfo>> | Promise<Resolved<ProfileInfo>> {
    var username = route.params['username'];
    return this.weShareClient.getProfileInfoByUsername(username)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<ProfileInfo>(error))),
      );
  }
}
