import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../services/weshareclient";
import { Resolved } from "../../../types/general-types";
import { ShareSecrets } from "../../../types/share-types";

@Injectable()
export class ViewShareSecretsResolver implements Resolve<Resolved<ShareSecrets>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<ShareSecrets> | Observable<Resolved<ShareSecrets>> | Promise<Resolved<ShareSecrets>> {
    var shareId = route.parent!.params['shareId'];
    return this.weShareClient.getShareSecrets(shareId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<ShareSecrets>(error))),
      );
  }
}
