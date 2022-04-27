import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { WeShareClient } from '../../../../../services/weshareclient';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent {
  errorCode: number = 0;
  callbackSecret: string;

  passwordForm: FormGroup;

  constructor(private router: Router, private weShareClient: WeShareClient,
    route: ActivatedRoute, formBuilder: FormBuilder) {
    route.params.subscribe(params => {
      this.callbackSecret = params['callbackSecret'];
      if (this.callbackSecret == null) {
        router.navigateByUrl("/");
        return;
      }
    });

    this.passwordForm = formBuilder.group({
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(64)]],
    });
  }

  submit() {
    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      return;
    }

    this.errorCode = 0;
    const val = this.passwordForm.value;

    this.weShareClient.resetPassword(this.callbackSecret, val.password)
      .subscribe(response => {
        this.router.navigateByUrl("/login");
      }, (error: HttpErrorResponse) => {
        this.errorCode = error.status;
      });
  }

  get password() {
    return this.passwordForm.get('password')!;
  }
}
