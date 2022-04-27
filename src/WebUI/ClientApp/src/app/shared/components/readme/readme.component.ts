import { Component, Input } from '@angular/core';
import { ShareSnippet } from '../../../../types/share-types';

@Component({
  selector: 'shared-readme',
  templateUrl: './readme.component.html',
  styleUrls: ['./readme.component.css']
})
export class ReadmeComponent {
  @Input()
  readme: string;


}
