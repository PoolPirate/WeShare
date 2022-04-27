import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { AuthService } from "../../../../services/authservice";
import { WeShareClient } from "../../../../services/weshareclient";
import { Resolved } from "../../../../types/general-types";
import { ProfileInfo } from "../../../../types/profile-types";


@Injectable()
export class UserSettingsProfileInfoResolver implements Resolve<Resolved<ProfileInfo>> {
  constructor(private weShareClient: WeShareClient, private authService: AuthService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<ProfileInfo> | Observable<Resolved<ProfileInfo>> | Promise<Resolved<ProfileInfo>> {
    const currentUserId = this.authService.getUserId()!;
    return this.weShareClient.getProfileInfoById(currentUserId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<ProfileInfo>(error))),
      );
  }
}
