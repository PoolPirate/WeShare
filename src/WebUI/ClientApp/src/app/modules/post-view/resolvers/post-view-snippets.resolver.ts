import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, concatMap, map } from "rxjs/operators";
import { AuthService } from "../../../../services/authservice";
import { WeShareClient } from "../../../../services/weshareclient";
import { Resolved } from "../../../../types/general-types";
import { PostSnippet } from "../../../../types/post-types";
import { ShareSnippet } from "../../../../types/share-types";

@Injectable()
export class PostViewSnippetsResolver implements Resolve<Resolved<[ShareSnippet, PostSnippet]>> {
  constructor(private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<[ShareSnippet, PostSnippet]> | Observable<Resolved<[ShareSnippet, PostSnippet]>> | Promise<Resolved<[ShareSnippet, PostSnippet]>> {
    const postId = route.params["postId"];
    
    return this.weShareClient.getPostSnippet(postId)
      .pipe(
        concatMap(postSnippet => {
          return this.weShareClient.getShareSnippet(postSnippet.shareId)
            .pipe(
              map(shareSnippet => {
                var result: [ShareSnippet, PostSnippet] = [shareSnippet, postSnippet];
                return Resolved.success(result);
              }),
              catchError(error => of(Resolved.error<[ShareSnippet, PostSnippet]>(error))),
            )
        }),
        catchError(error => of(Resolved.error<[ShareSnippet, PostSnippet]>(error))),
      );
  }
}
