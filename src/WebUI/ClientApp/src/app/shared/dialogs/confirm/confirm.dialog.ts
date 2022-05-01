import { Component, Inject } from "@angular/core";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
  selector: 'shared-confirm-dialog',
  templateUrl: './confirm.dialog.html',
  styleUrls: ['./confirm.dialog.scss']
})
export class SharedConfirmDialog {
  constructor(private dialogRef: MatDialogRef<SharedConfirmDialog>, dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) private parameters: Params) {
    if (dialog.openDialogs.some(x => x.componentInstance.constructor.name == SharedConfirmDialog.name && x.id != dialogRef.id)) {
      dialogRef.close(false);
    }
  }

  accept() {
    this.dialogRef.close(true);
  }

  reject() {
    this.dialogRef.close(false);
  }

  get title() {
    return this.parameters.title;
  }

  get description() {
    return this.parameters.description;
  }
}

interface Params {
  title: string;
  description: string;
}
