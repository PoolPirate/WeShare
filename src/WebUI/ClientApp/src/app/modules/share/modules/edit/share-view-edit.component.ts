import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';

@Component({
  selector: 'share-view-edit',
  templateUrl: './share-view-edit.component.html'
})
export class ShareViewEditComponent {
  constructor(route: ActivatedRoute, router: Router, authService: AuthService)
  {
    route.url.subscribe(url => {
      if (authService.isLoggedOut()) {
        router.navigate(["forbidden"]);
      }
    });
  }
}
