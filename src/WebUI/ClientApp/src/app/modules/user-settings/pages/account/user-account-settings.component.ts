import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';
import { WeShareClient } from '../../../../services/weshareclient';

@Component({
  selector: 'user-account-settings',
  templateUrl: './user-account-settings.component.html',
  styleUrls: ['./user-account-settings.component.css']
})
export class UserAccountSettingsComponent {
  usernameForm: FormGroup;

  ranForUsername: string = "";
  errorCode: number = 0;

  constructor(private weShareClient: WeShareClient, private authService: AuthService,
    formBuilder: FormBuilder, router: Router) {

    this.usernameForm = formBuilder.group({
      username: [authService.getUsername(), [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
    });
  }

  updateUsername() {
    const oldErrorCode = this.errorCode;
    this.errorCode = 0;

    if (this.usernameForm.invalid) {
      this.usernameForm.markAllAsTouched();
      return;
    }

    const val = this.usernameForm.value;
    const username = val.username.toLowerCase();

    if (this.authService.getUsername() == username) { this.errorCode = 200; return; }
    if (this.ranForUsername == username) { this.errorCode = oldErrorCode; return; }

    this.ranForUsername = username;

    this.weShareClient.updateAccount(username, null)
      .subscribe(response => {
        this.errorCode = 200;
        this.authService.setUsername(username);
        return;
      }, (error: HttpErrorResponse) => {
        this.errorCode = error.status;
      });
  }

  get username() {
    return this.usernameForm.get("username")!;
  }
}
