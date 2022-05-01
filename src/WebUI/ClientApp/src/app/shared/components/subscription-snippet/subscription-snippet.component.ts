import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DialogService } from '../../../../services/dialogservice';
import { SubscriptionSnippet } from '../../../../types/subscription-types';

@Component({
  selector: 'shared-subscription-snippet',
  templateUrl: './subscription-snippet.component.html',
  styleUrls: ['./subscription-snippet.component.css']
})
export class SubscriptionSnippetComponent {
  constructor(private dialogService: DialogService) { }

  @Input()
  subscriptionSnippet: SubscriptionSnippet;

  @Output()
  delete = new EventEmitter();

  async onDelete() {
    if (await this.dialogService.confirm("Delete Subscription?", "You cannot undo this action")) {
      this.delete.emit();
    }
  }
}
