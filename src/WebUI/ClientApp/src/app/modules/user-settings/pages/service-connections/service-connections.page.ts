import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WeShareClient } from '../../../../../services/weshareclient';
import { ServiceConnectionSnippet } from '../../../../../types/account-types';
import { PaginatedResponse, Resolved } from '../../../../../types/general-types';

@Component({
  selector: 'service-connections',
  templateUrl: './service-connections.page.html',
  styleUrls: ['./service-connections.page.css']
})
export class ServiceConnectionsPage {
  errorCode: number;
  
  serviceConnectionsResponse: PaginatedResponse<ServiceConnectionSnippet>;
  serviceConnections: ServiceConnectionSnippet[];

  constructor(private weShareClient: WeShareClient, private router: Router, route: ActivatedRoute) {
    route.data.subscribe(data => {
      const serviceConnectionsResponse: Resolved<PaginatedResponse<ServiceConnectionSnippet>> = data.serviceConnectionsResponse;

      if (serviceConnectionsResponse.ok) {
        this.serviceConnectionsResponse = serviceConnectionsResponse.content!;
        this.serviceConnections = this.serviceConnectionsResponse.items;
      } else {
        if (serviceConnectionsResponse.status == 404) {
          router.navigateByUrl("/");
          return;
        }

        this.errorCode = serviceConnectionsResponse.status;
        return;
      }
    });
  }

  onDelete(serviceConnectionSnippet: ServiceConnectionSnippet) {
    this.weShareClient.removeServiceConnection(serviceConnectionSnippet.id)
      .subscribe(success => {
      }, error => {
        alert("There was an error while deleting service connection");
      });
  }

  redirectToDiscord() {
    //window.location.href = "https://discord.com/api/oauth2/authorize?client_id=971200529013309461&redirect_uri=https%3A%2F%2Flocalhost%3A44440%2Fc%2Fdiscord-callback&response_type=code&scope=identify%20guilds.join";
    window.location.href = "https://discord.com/api/oauth2/authorize?client_id=971200529013309461&redirect_uri=https%3A%2F%2Fwe-share-live.de%2Fc%2Fdiscord-callback&response_type=code&scope=identify%20guilds.join";
  }
}
