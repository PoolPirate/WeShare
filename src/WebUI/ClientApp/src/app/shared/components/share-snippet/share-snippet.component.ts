import { Component, Input } from '@angular/core';
import { ShareSnippet } from '../../../../types/share-types';
import { UserSnippet } from '../../../../types/user-types';

@Component({
  selector: 'shared-share-snippet',
  templateUrl: './share-snippet.component.html',
  styleUrls: ['./share-snippet.component.css']
})
export class ShareSnippetComponent {
  @Input()
  shareSnippet: ShareSnippet;
}
