import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { AuthService } from "../../../../services/authservice";
import { WeShareClient } from "../../../../services/weshareclient";
import { Resolved } from "../../../../types/general-types";
import { PostContent } from "../../../../types/post-types";

@Injectable()
export class PostViewPostContentResolver implements Resolve<Resolved<PostContent | null>> {
  constructor(private authService: AuthService, private weShareClient: WeShareClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Resolved<PostContent | null> | Observable<Resolved<PostContent | null>> | Promise<Resolved<PostContent | null>> {
    const postId = route.params["postId"];
    return this.weShareClient.getPostContent(postId)
      .pipe(
        map(value => (Resolved.success(value))),
        catchError(error => of(Resolved.error<PostContent>(error))),
      );
  }
}
