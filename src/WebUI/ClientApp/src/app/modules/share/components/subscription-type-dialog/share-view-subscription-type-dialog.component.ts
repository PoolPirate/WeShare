import { Component, Inject } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { WeShareClient } from "../../../../../services/weshareclient";
import { SubscriptionType } from "../../../../../types/subscription-types";

@Component({
  selector: 'share-view-subscription-type-dialog',
  templateUrl: './share-view-subscription-type-dialog.component.html',
  styleUrls: ['./share-view-subscription-type-dialog.component.scss']
})
export class ShareViewSubscriptionTypeDialogComponent {
  submitRequest: Observable<number> | null;

  subscriptionType = SubscriptionType;

  constructor(private router: Router, private weShareClient: WeShareClient, private dialogRef: MatDialogRef<ShareViewSubscriptionTypeDialogComponent>,
    formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: SubscriptionTypeDialogData) {
  }

  createAndRedirect(type: SubscriptionType) {
    if (this.submitRequest) {
      return;
    }

    this.submitRequest = this.weShareClient.createSubscription(this.data.shareId, type, "New");

    this.submitRequest.subscribe(success => {
      this.router.navigate(['/subscription', success]);
      this.dialogRef.close();
    }, error => {
      alert("Faild: The server responded with: " + error.status);
    }, () => {
      this.submitRequest = null;
    });
  }
}

export interface SubscriptionTypeDialogData {
  shareId: number;
}
