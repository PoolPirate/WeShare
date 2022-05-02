import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';
import { WeShareClient } from '../../../../../services/weshareclient';

@Component({
  selector: 'app-request-reset-component',
  templateUrl: './request-reset.component.html'
})
export class RequestResetComponent {
  resetForm: FormGroup;

  errorCode: number = 0;

  ranFor: string = "";

  constructor(private weShareClient: WeShareClient,
    formBuilder: FormBuilder, authService: AuthService, router: Router) {

    if (authService.isLoggedIn()) {
      router.navigateByUrl("");
    }

    this.resetForm = formBuilder.group({
      email: ['', [Validators.required, Validators.maxLength(128), Validators.email]],
    });
  }

  reset() {
    this.errorCode = 0;

    if (this.resetForm.invalid) {
      this.resetForm.markAllAsTouched();
      return;
    }

    const val = this.resetForm.value;
    const email: string = val.email;

    if (this.ranFor.toUpperCase() == email.toUpperCase()) { return; }

    this.ranFor = email;

    this.weShareClient.requestPasswordReset(email)
      .subscribe(response => {
        this.errorCode = 200;
      }, (error: HttpErrorResponse) => {
        this.errorCode = error.status;
      });
  }

  get email() {
    return this.resetForm.get('email')!;
  }
}
