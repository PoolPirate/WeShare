import { Component, Input, Output, EventEmitter } from '@angular/core';
import { SubscriptionSnippet } from '../../../../types/subscription-types';

@Component({
  selector: 'shared-subscription-snippet',
  templateUrl: './subscription-snippet.component.html',
  styleUrls: ['./subscription-snippet.component.css']
})
export class SubscriptionSnippetComponent {
  @Input()
  subscriptionSnippet: SubscriptionSnippet;

  @Output()
  delete = new EventEmitter();

  onDelete() {
    this.delete.emit();
  }
}
