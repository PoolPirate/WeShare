import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WeShareClient } from '../../../../../services/weshareclient';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.css']
})
export class VerifyEmailComponent {
  errorCode: number = 0;

  constructor(route: ActivatedRoute, router: Router, weShareClient: WeShareClient) {
    route.params.subscribe(params => {
      var callbackSecret = params['callbackSecret'];
      if (callbackSecret == null) {
        console.info("No callbackSecret!");
        router.navigateByUrl("/");
        return;
      }

      weShareClient.handleVerifyEmailCallback(callbackSecret)
        .subscribe(response => {
          this.errorCode = 0;
        }, (error: HttpErrorResponse) => {
          this.errorCode = error.status;
          router.navigateByUrl("/notfound");
        })
    });
  }
}
