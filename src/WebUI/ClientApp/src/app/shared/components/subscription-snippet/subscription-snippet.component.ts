import { Component, Input } from '@angular/core';
import { SubscriptionSnippet } from '../../../../types/subscription-types';

@Component({
  selector: 'subscription-snippet',
  templateUrl: './subscription-snippet.component.html',
  styleUrls: ['./subscription-snippet.component.css']
})
export class SubscriptionSnippetComponent {
  @Input()
  subscriptionSnippet: SubscriptionSnippet;
}
