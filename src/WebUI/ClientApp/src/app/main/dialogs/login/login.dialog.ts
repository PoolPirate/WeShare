import { HttpErrorResponse } from "@angular/common/http";
import { Component } from "@angular/core";
import { FormControl, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { AuthService } from "../../../../services/authservice";

@Component({
  selector: 'login-dialog',
  templateUrl: './login.dialog.html',
  styleUrls: ['./login.dialog.css']
})
export class LoginDialog {
  username: FormControl;
  password: FormControl;

  ranForUsername: string = "";
  ranForPassword: string = "";
  errorCode: number = 0;

  constructor(private authService: AuthService,
    private dialogRef: MatDialogRef<LoginDialog>) {
    this.username = new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]);
    this.password = new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(64)]);

    if (authService.isLoggedIn()) {
      this.dialogRef.close();
    }
  }

  submitLogin() {
    this.username.markAllAsTouched();
    this.password.markAllAsTouched();

    if (this.username.invalid || this.password.invalid) {
      return;
    }

    const username = this.username.value;
    const password = this.password.value;

    if (this.ranForUsername.toUpperCase() == username.toUpperCase() && this.ranForPassword == password) {
      return;
    }

    this.errorCode = 0;

    this.ranForUsername = username;
    this.ranForPassword = password;

    this.authService.login(username, password)
      .subscribe(response => {
        this.dialogRef.close(true);
      }, (error: HttpErrorResponse) => {
        this.errorCode = error.status;
      });
  }
}
