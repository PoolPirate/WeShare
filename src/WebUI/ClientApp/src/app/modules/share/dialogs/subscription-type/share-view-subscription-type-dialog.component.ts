import { ComponentType } from "@angular/cdk/portal";
import { Component, Inject } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { DialogService } from "../../../../../services/dialogservice";
import { WeShareClient } from "../../../../../services/weshareclient";
import { SubscriptionType } from "../../../../../types/subscription-types";
import { ShareViewCreateDashboardSubscriptionDialog } from "../subscription-create/dashboard/create-dashboard-subscription.dialog";
import { ShareViewCreateDiscordSubscriptionDialog } from "../subscription-create/discord/create-discord-subscription.dialog";
import { ShareViewCreateWebhookSubscriptionDialog } from "../subscription-create/webhook/create-webhook-subscription.dialog";

@Component({
  selector: 'share-view-subscription-type-dialog',
  templateUrl: './share-view-subscription-type-dialog.component.html',
  styleUrls: ['./share-view-subscription-type-dialog.component.scss', '../share-view.dialog.css']
})
export class ShareViewSubscriptionTypeDialogComponent {
  submitRequest: Observable<number> | null;

  subscriptionType = SubscriptionType;

  shareId: number;

  constructor(private router: Router, private weShareClient: WeShareClient, private dialog: MatDialog,
    private dialogRef: MatDialogRef<ShareViewSubscriptionTypeDialogComponent>, private dialogService: DialogService,
    @Inject(MAT_DIALOG_DATA) public data: SubscriptionTypeDialogData) {
    this.shareId = data.shareId;
  }

  openCreationDialog(type: SubscriptionType) {
    switch (type) {
      case SubscriptionType.Dashboard:
        this.exitWithDialog(ShareViewCreateDashboardSubscriptionDialog, { shareId: this.shareId });
        return;

      case SubscriptionType.Webhook:
        this.exitWithDialog(ShareViewCreateWebhookSubscriptionDialog, { shareId: this.shareId });
        return;

      case SubscriptionType.MessagerDiscord:
        this.dialogService.openLoading();

        this.weShareClient.getDiscordServiceConnections()
          .subscribe(success => {
            this.exitWithDialog(ShareViewCreateDiscordSubscriptionDialog, { shareId: this.shareId, discordConnections: success });
          }, error => { }, () => {
            this.dialogService.closeLoading();
          })


        return;
    }
  }

  exitWithDialog<T>(component: ComponentType<T>, data: Object) {
    this.dialogRef.close();
    this.dialog.open(component, {
      data: data
    });
  }
}

export interface SubscriptionTypeDialogData {
  shareId: number;
}
