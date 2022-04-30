import { ComponentType } from "@angular/cdk/portal";
import { STEPPER_GLOBAL_OPTIONS } from "@angular/cdk/stepper";
import { Component, Inject, ViewChild } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatStep, MatStepper } from "@angular/material/stepper";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { WeShareClient } from "../../../../../../services/weshareclient";
import { SubscriptionType } from "../../../../../../types/subscription-types";
import { ShareViewSubscriptionTypeDialogComponent } from "../../subscription-type/share-view-subscription-type-dialog.component";

@Component({
  selector: 'create-dashboard-subscription-dialog',
  templateUrl: './create-dashboard-subscription.dialog.html',
  styleUrls: ['./create-dashboard-subscription.dialog.scss', '../../share-view.dialog.css'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { showError: true },
    }
  ]
})
export class ShareViewCreateDashboardSubscriptionDialog {
  name: FormControl;

  submitRequest: Observable<number> | null;

  @ViewChild(MatStepper) stepper: MatStepper

  constructor(private router: Router, private weShareClient: WeShareClient, private dialog: MatDialog,
    private dialogRef: MatDialogRef<ShareViewCreateDashboardSubscriptionDialog>, 
    formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: SubscriptionTypeDialogData) {

    this.name = new FormControl("", [Validators.required, Validators.minLength(1), Validators.maxLength(24)]);
  }

  createAndRedirect() {
    if (this.submitRequest) {
      return;
    }
    if (!this.stepperCompleted) {
      return;
    }

    this.submitRequest = this.weShareClient.createDashboardSubscription(this.data.shareId, this.name.value);

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

  backToTypeSelect() {
    this.dialogRef.close();
    this.dialog.open(ShareViewSubscriptionTypeDialogComponent, {
      data: { shareId: this.data.shareId }
    });
  }
}

export interface SubscriptionTypeDialogData {
  shareId: number;
}
