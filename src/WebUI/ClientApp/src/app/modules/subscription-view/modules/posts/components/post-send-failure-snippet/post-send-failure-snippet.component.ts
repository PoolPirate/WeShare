import { Component, Input } from '@angular/core';
import { PostSendFailure, WebhookPostSendFailure } from '../../../../../../../types/post-types';
import { SubscriptionType } from '../../../../../../../types/subscription-types';

@Component({
  selector: 'post-send-failure-snippet',
  templateUrl: './post-send-failure-snippet.component.html',
  styleUrls: ['./post-send-failure-snippet.component.css']
})
export class PostSendFailureSnippetComponent {
  type = SubscriptionType;


  @Input()
  subscriptionType: SubscriptionType;

  @Input()
  postSendFailure: PostSendFailure;

  get webhook() {
    return this.postSendFailure as WebhookPostSendFailure;
  }
}
