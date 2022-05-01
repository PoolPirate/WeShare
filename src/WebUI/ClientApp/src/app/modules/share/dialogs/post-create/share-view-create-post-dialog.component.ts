import { Component, Inject } from "@angular/core";
import { FormControl, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { WeShareClient } from "../../../../../services/weshareclient";
import { ShareSecrets } from "../../../../../types/share-types";
import { ShareService } from "../../services/shareservice";

@Component({
  selector: 'share-view-create-post-dialog',
  templateUrl: './share-view-create-post-dialog.component.html',
  styleUrls: ['./share-view-create-post-dialog.component.scss']
})
export class ShareViewCreatePostDialogComponent {
  headers: [string, string][] = [];

  key: FormControl;
  value: FormControl;

  payload: FormControl;

  constructor(private weShareClient: WeShareClient,
    @Inject(MAT_DIALOG_DATA) public shareSecrets: ShareSecrets) {
    this.key = new FormControl("", [Validators.required]);
    this.value = new FormControl("", [Validators.required]);

    this.payload = new FormControl("");
  }

  addToHeaderList() {
    this.key.markAllAsTouched();
    this.value.markAllAsTouched();

    if (!this.headerInputsValid()) {
      return;
    }

    if (this.headers.some(x => x[0] == this.key.value)) {
      return;
    }

    this.headers.push([this.key.value, this.value.value]);
  }

  headerInputsValid() {
    return this.key.valid && this.value.valid;
  }

  submit() {
    var payloadRaw = new TextEncoder().encode("¢");
    this.weShareClient.submitPost(this.shareSecrets.secret, this.headers, payloadRaw)
      .subscribe(success => {
        alert("Submitted!");
      }, error => {
        alert("Response was " + error.status);
      });
  }
}
