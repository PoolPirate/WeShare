import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DialogService } from '../../../../../services/dialogservice';
import { DiscordServiceConnectionSnippet, ServiceConnectionSnippet, ServiceConnectionType } from '../../../../../types/account-types';

@Component({
  selector: 'service-connection-snippet',
  templateUrl: './service-connection-snippet.component.html',
  styleUrls: ['./service-connection-snippet.component.css']
})
export class ServiceConnectionSnippetComponent {
  constructor(private dialogService: DialogService) { }

  @Input()
  serviceConnectionSnippet: ServiceConnectionSnippet;

  @Output()
  delete = new EventEmitter<ServiceConnectionSnippet>();

  async onDelete() {
    if (await this.dialogService.confirm("Remove Service Link?", "You can always add it later")) {
      this.delete.emit(this.serviceConnectionSnippet);
    }
  }

  get serviceConnectionType() {
    return this.serviceConnectionSnippet.type;
  }

  get isDiscord() {
    return this.serviceConnectionType == ServiceConnectionType.Discord;
  }

  get discord() {
    return this.serviceConnectionSnippet as DiscordServiceConnectionSnippet;
  }
}
