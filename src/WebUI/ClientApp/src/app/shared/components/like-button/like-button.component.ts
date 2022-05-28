import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'shared-like-button',
  templateUrl: './like-button.component.html',
  styleUrls: ['./like-button.component.scss']
})
export class LikeButtonComponent {
  @Input()
  liked: boolean = false;

  @Output()
  onLike = new EventEmitter<boolean>();

  like() {
    this.onLike.emit(this.liked);
  }
}
