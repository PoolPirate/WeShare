import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResponse, Resolved } from '../../types/general-types';
import { SubscriptionInfo } from '../../types/subscription-types';
import { DashboardService } from './services/dashboardservice';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  providers: [DashboardService]
})
export class DashboardComponent {
  constructor() {

  }
}
