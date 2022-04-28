import { Component, Input } from '@angular/core';
import { PostSnippet } from '../../../../types/post-types';

@Component({
  selector: 'shared-post-snippet',
  templateUrl: './post-snippet.component.html',
  styleUrls: ['./post-snippet.component.css']
})
export class PostSnippetComponent {
  @Input()
  postSnippet: PostSnippet;
}
