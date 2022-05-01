import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../../services/authservice';

@Component({
  selector: 'subscription-view-posts',
  templateUrl: './subscription-view-posts.component.html'
})
export class SubscriptionViewPostsComponent {
  constructor(route: ActivatedRoute, router: Router, authService: AuthService)
  {
  }
}
