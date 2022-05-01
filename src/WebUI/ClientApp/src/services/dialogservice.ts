import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { SharedConfirmDialog } from "../app/shared/dialogs/confirm/confirm.dialog";
import { ComponentType } from "@angular/cdk/portal";

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


  private hasDialog<T>(component: ComponentType<T>): boolean {
    return this.dialog.openDialogs
      .some(x => x.componentInstance.constructor.name == SharedConfirmDialog.name)
  }
}
