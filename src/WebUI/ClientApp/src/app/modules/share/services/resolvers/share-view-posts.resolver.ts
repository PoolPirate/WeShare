import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { WeShareClient } from "../../../../../services/weshareclient";
import { PaginatedResponse, Resolved } from "../../../../../types/general-types";
import { PostOrdering, PostSnippet } from "../../../../../types/post-types";

@Injectable()
export class ViewSharePostsResolver implements Resolve<Resolved<PaginatedResponse<PostSnippet>>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PaginatedResponse<PostSnippet>> | Observable<Resolved<PaginatedResponse<PostSnippet>>> | Promise<Resolved<PaginatedResponse<PostSnippet>>> {
    var shareId = route.parent!.params['shareId'];

    var ordering = parseInt(route.queryParams['order']);

    if (!Object.values(PostOrdering).includes(ordering)) {
      ordering = PostOrdering.CreatedAtDesc;
    }

    return this.weShareClient.getPosts(shareId, ordering, 0, 10)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PaginatedResponse<PostSnippet>>(error))),
      );
  }
}
