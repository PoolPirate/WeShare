import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { AuthService } from "../../../../services/authservice";
import { WeShareClient } from "../../../../services/weshareclient";
import { AccountInfo } from "../../../../types/account-types";
import { Resolved } from "../../../../types/general-types";

@Injectable()
export class UserSettingsAccountInfoResolver implements Resolve<Resolved<AccountInfo>> {
  constructor(private weShareClient: WeShareClient, private authService: AuthService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<AccountInfo> | Observable<Resolved<AccountInfo>> | Promise<Resolved<AccountInfo>> {
    const currentUserId = this.authService.getUserId()!;
    return this.weShareClient.getAccountInfo(currentUserId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<AccountInfo>(error))),
      );
  }
}
