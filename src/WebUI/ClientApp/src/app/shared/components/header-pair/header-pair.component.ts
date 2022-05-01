import { Component, Input } from '@angular/core';

@Component({
  selector: 'shared-header-pair',
  templateUrl: './header-pair.component.html',
  styleUrls: ['./header-pair.component.css']
})
export class HeaderPairComponent {
  @Input()
  key: string;

  @Input()
  value: string;
}
