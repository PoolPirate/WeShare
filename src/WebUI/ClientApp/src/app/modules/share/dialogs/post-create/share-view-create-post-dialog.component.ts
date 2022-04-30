import { Component } from "@angular/core";

@Component({
  selector: 'share-view-create-post-dialog',
  templateUrl: './share-view-create-post-dialog.component.html',
  styleUrls: ['./share-view-create-post-dialog.component.scss']
})
export class ShareViewCreatePostDialogComponent {
  submit() {
    alert("Submitted");
  }
}
