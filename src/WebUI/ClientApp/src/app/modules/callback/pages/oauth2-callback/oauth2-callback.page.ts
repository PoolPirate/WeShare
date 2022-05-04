import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';
import { WeShareClient } from '../../../../../services/weshareclient';
import { ServiceConnectionType } from '../../../../../types/account-types';

@Component({
  selector: 'oauth2-callback',
  templateUrl: './oauth2-callback.page.html',
  styleUrls: ['./oauth2-callback.page.css']
})
export class OAuth2CallbackPage {
  errorCode: number = 0;
  code: string | null;

  constructor(private router: Router, private weShareClient: WeShareClient, private authService: AuthService,
    route: ActivatedRoute) {

    if (this.authService.isLoggedOut()) {
      router.navigate(["login"]);
      return;
    }

    route.queryParams.subscribe(queryParams => {
      this.code = queryParams["code"];

      if (this.code == null || this.code == "") {
        alert("Missing grant code!");
        this.router.navigate(["/"]);
      }

      this.createServiceConnection();
    });
  }

  createServiceConnection() {
    this.weShareClient.createServiceConnection(ServiceConnectionType.Discord, this.code!)
      .subscribe(success => {
        this.router.navigate(["/", "user", "settings", "connections"]);
      }, error => {
        alert("There was an error creating your service connection: The server responded with: " + error.status);
        this.router.navigate(["/"]);
      });
  }
}
