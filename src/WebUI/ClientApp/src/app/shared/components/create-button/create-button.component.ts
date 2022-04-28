import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'shared-create-button',
  templateUrl: './create-button.component.html',
  styleUrls: ['./create-button.component.scss']
})
export class CreateButtonComponent {
  @Output()
  onCreate = new EventEmitter();

  create() {
    this.onCreate.emit(null);
  }
}
