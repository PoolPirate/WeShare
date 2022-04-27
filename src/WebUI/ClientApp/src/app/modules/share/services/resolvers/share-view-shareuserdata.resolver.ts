import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../../services/weshareclient";
import { Resolved } from "../../../../../types/general-types";
import { ShareUserData } from "../../../../../types/share-types";


@Injectable()
export class ViewShareShareUserDataResolver implements Resolve<Resolved<ShareUserData> | null> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<ShareUserData> | null | Observable<Resolved<ShareUserData>> | Promise<Resolved<ShareUserData>> {
    var shareId = route.params['shareId'];
    return this.weShareClient.getShareUserData(shareId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<ShareUserData>(error))),
      );
  }
}
