import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ShareService } from "../../services/shareservice";

@Component({
  selector: 'share-view-subscribe-button',
  templateUrl: './share-view-subscribe-button.component.html',
  styleUrls: ['./share-view-subscribe-button.component.scss']
})
export class ShareViewSubscribeButtonComponent {
  @Input()
  subscribed: boolean;

  @Output()
  onStatusUpdate = new EventEmitter<boolean>();

  subscribe() {
    this.onStatusUpdate.emit(this.subscribed);
  }
}
