import { ComponentType } from "@angular/cdk/portal";
import { STEPPER_GLOBAL_OPTIONS } from "@angular/cdk/stepper";
import { Component, Inject, ViewChild } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatStepper } from "@angular/material/stepper";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { WeShareClient } from "../../../../../../services/weshareclient";
import { DiscordServiceConnectionSnippet } from "../../../../../../types/account-types";
import { ShareViewSubscriptionTypeDialogComponent } from "../../subscription-type/share-view-subscription-type-dialog.component";

@Component({
  selector: 'create-discord-subscription-dialog',
  templateUrl: './create-discord-subscription.dialog.html',
  styleUrls: ['./create-discord-subscription.dialog.scss', '../../share-view.dialog.css'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { showError: true },
    }
  ]
})
export class ShareViewCreateDiscordSubscriptionDialog {
  name: FormControl;
  serviceConnectionId: FormControl;

  submitRequest: Observable<number> | null;

  @ViewChild(MatStepper) stepper: MatStepper

  constructor(private router: Router, private weShareClient: WeShareClient, private dialog: MatDialog,
    private dialogRef: MatDialogRef<ShareViewCreateDiscordSubscriptionDialog>,
    formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: CreateDiscordSubscriptionDialogData) {

    this.name = new FormControl("", [Validators.required, Validators.minLength(1), Validators.maxLength(24)]);
    this.serviceConnectionId = new FormControl("", [Validators.required]);
  }

  createAndRedirect() {
    if (this.submitRequest) {
      return;
    }
    if (!this.stepperCompleted) {
      return;
    }
    console.log(this);
    this.submitRequest = this.weShareClient.createDiscordSubscription(this.data.shareId, this.name.value, this.serviceConnectionId.value);

    this.submitRequest.subscribe(success => {
      this.router.navigate(['/subscription', success]);
      this.dialogRef.close();
    }, error => {
      alert("Faild: The server responded with: " + error.status);
    }, () => {
      this.submitRequest = null;
    });
  }

  get stepperCompleted(): boolean {
    return this.name.valid &&
      this.stepper.steps.find(x => x.hasError) == null;
  }

  get discordConnections() {
    return this.data.discordConnections;
  }

  backToTypeSelect() {
    this.dialogRef.close();
    this.dialog.open(ShareViewSubscriptionTypeDialogComponent, {
      data: { shareId: this.data.shareId }
    });
  }
}

export interface CreateDiscordSubscriptionDialogData {
  shareId: number;
  discordConnections: DiscordServiceConnectionSnippet[];
}
