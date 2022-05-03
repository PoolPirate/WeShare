import { STEPPER_GLOBAL_OPTIONS } from "@angular/cdk/stepper";
import { Component } from "@angular/core";
import { FormControl, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { Router } from "@angular/router";
import { WeShareClient } from "../../../../../services/weshareclient";

@Component({
  selector: 'share-create-dialog',
  templateUrl: './share-create.dialog.html',
  styleUrls: ['./share-create.dialog.scss'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { showError: true },
    }
  ],
})
export class ShareCreateDialog {
  name: FormControl;
  description: FormControl;
  readme: FormControl;

  requestRunning: boolean = false;

  constructor(private weShareClient: WeShareClient, private dialogRef: MatDialogRef<ShareCreateDialog>, private router: Router) {
    this.name = new FormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(32), Validators.pattern("([a-zA-Z0-9])*")]);
    this.description = new FormControl("", [Validators.required, Validators.maxLength(256)]);
    this.readme = new FormControl("", [Validators.maxLength(4096)]);
  }

  submit() {
    this.name.markAllAsTouched();
    this.description.markAllAsTouched();
    this.readme.markAllAsTouched();

    if (this.name.invalid || this.description.invalid || this.readme.invalid) {
      return;
    }
    if (this.requestRunning) {
      return;
    }

    this.requestRunning = true;

    this.weShareClient.createShare(this.name.value, this.description.value, this.readme.value, false)
      .subscribe(success => {
        this.router.navigate(["/", "share", "view", success]);
        this.dialogRef.close();
      }, error => {
        alert("The server responded with: " + error.status);
        this.requestRunning = false;
      });
  }
}
