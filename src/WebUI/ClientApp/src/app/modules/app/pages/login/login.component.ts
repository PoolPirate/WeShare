import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';


@Component({
  selector: 'app-login-component',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;

  ranForUsername: string = "";
  ranForPassword: string = "";
  errorCode: number = 0;

  constructor(private authService: AuthService, private router: Router,
    formBuilder: FormBuilder,) {

    if (authService.isLoggedIn()) {
      router.navigateByUrl("/");
    }

    this.loginForm = formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(64)]]
    });
  }

  login() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.errorCode = 0;
    const val = this.loginForm.value;
    const username = val.username;
    const password = val.password;

    if (this.ranForUsername.toUpperCase() == username.toUpperCase() && this.ranForPassword == password) { return; }

    this.ranForUsername = username;
    this.ranForPassword = password;

    this.authService.login(val.username, val.password)
      .subscribe(response => {
        this.router.navigateByUrl("/");
        return;
      }, (error: HttpErrorResponse) => {
        this.errorCode = error.status;
      });
  }

  get username() {
    return this.loginForm.get("username")!;
  }
  get password() {
    return this.loginForm.get("password")!;
  }
}
