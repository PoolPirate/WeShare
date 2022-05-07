import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { SharedConfirmDialog } from "../app/shared/dialogs/confirm/confirm.dialog";
import { ComponentType } from "@angular/cdk/portal";
import { SharedLoadingDialog } from "../app/shared/dialogs/loading/loading.dialog.component";

@Injectable()
export class DialogService {
  constructor(private dialog: MatDialog) {
  }

  async confirm(title: string, description: string): Promise<boolean> {
    if (this.hasDialog(SharedConfirmDialog)) {
      return false;
    }

    return this.dialog.open(SharedConfirmDialog, {
      data: { title, description }
    })
      .afterClosed()
      .toPromise();
  }

  openLoading() {
    if (this.hasDialog(SharedLoadingDialog)) {
      return;
    }

    this.dialog.open(SharedLoadingDialog);
  }
  closeLoading() {
    this.closeDialog(SharedLoadingDialog);
  }

  private hasDialog<T>(component: ComponentType<T>): boolean {
    return this.dialog.openDialogs
      .some(x => x.componentInstance.constructor.name == component.name)
  }
  private closeDialog<T>(component: ComponentType<T>) {
    const dialog = this.dialog.openDialogs.find(x => x.componentInstance.constructor.name == component.name);
    dialog?.close();
  }
}
