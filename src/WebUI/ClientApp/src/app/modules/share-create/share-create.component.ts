import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/authservice';
import { WeShareClient } from '../../services/weshareclient';

@Component({
  selector: 'share-create',
  templateUrl: './share-create.component.html'
})
export class ShareCreateComponent {
  shareName: string = "";
  shareDescription: string = "";
  shareReadme: string = "";

  step: 0 | 1 = 0;

  errorCode: Number = 0;

  constructor(authService: AuthService, private router: Router,
    private weShareClient: WeShareClient) {

    if (authService.isLoggedOut()) {
      return;
    }
  }

  submitBasic(params: [string, string]) {
    [this.shareName, this.shareDescription] = params;
    this.step = 1;
  }
  submitReadme(readme: string) {
    this.shareReadme = readme;
    this.createShare();
  }

  createShare() {
    this.weShareClient.createShare(this.shareName, this.shareDescription, this.shareReadme)
      .subscribe(response => {
        var shareId = response;
        this.router.navigateByUrl("/share/view/" + shareId);
      }, (error: HttpErrorResponse) => {
        alert("The server responded with " + error.status);
      });
  }
}
