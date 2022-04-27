import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../services/weshareclient";
import { Resolved } from "../../../types/general-types";
import { ShareData, ShareInfo } from "../../../types/share-types";

@Injectable()
export class ViewShareShareInfoResolver implements Resolve<Resolved<ShareData>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<ShareData> | Observable<Resolved<ShareData>> | Promise<Resolved<ShareData>> {
    var shareId = route.params['shareId'];
    return this.weShareClient.getShareInfo(shareId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<ShareData>(error))),
      );
  }
}
