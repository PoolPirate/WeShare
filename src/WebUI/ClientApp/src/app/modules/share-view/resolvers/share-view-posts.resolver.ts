import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../types/general-types";
import { PostMetadata } from "../../../types/post-types";

@Injectable()
export class ViewSharePostsResolver implements Resolve<Resolved<PaginatedResponse<PostMetadata>>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<PostMetadata>> | Observable<Resolved<PaginatedResponse<PostMetadata>>> | Promise<Resolved<PaginatedResponse<PostMetadata>>> {
    var shareId = route.parent!.params['shareId'];

    return this.weShareClient.getPosts(shareId, 0)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<PostMetadata>>(error))),
      );
  }
}
