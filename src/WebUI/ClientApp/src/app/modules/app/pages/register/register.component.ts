import { useAnimation } from '@angular/animations';
import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';
import { WeShareClient } from '../../../../services/weshareclient';

@Component({
  selector: 'app-register-component',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;

  ranForUsername: string = "";
  ranForEmail: string = "";
  ranForPassword: string = "";
  errorCode: number = 0;

  constructor(private fb: FormBuilder, private router: Router, private weShareClient: WeShareClient,
    authService: AuthService) {
    if (authService.isLoggedIn()) {
      router.navigateByUrl("");
    }

    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.maxLength(128), Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(64)]],
    });
  }

  register() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.errorCode = 0;
    const val = this.registerForm.value;
    const username = val.username;
    const email = val.email;
    const password = val.password;

    if (this.ranForUsername.toUpperCase() == username.toUpperCase() &&
      this.ranForEmail.toUpperCase() == email.toUpperCase() &&
      this.ranForPassword == password) { return; }

    this.ranForUsername = username;
    this.ranForEmail = email;
    this.ranForPassword = password;

    this.weShareClient.createUser(val.username, val.email, val.password)
      .subscribe(response => {
        this.router.navigateByUrl("/login");
      }, (error: HttpErrorResponse) => {
        this.errorCode = error.status;
      });
  }

  get password() {
    return this.registerForm.get('password')!;
  }
  get username() {
    return this.registerForm.get('username')!;
  }
  get email() {
    return this.registerForm.get('email')!;
  }
}
