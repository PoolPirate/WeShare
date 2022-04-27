import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/authservice';

@Component({
  selector: 'user-settings',
  templateUrl: './user-settings.component.html'
})
export class UserSettingsComponent {
  constructor(authService: AuthService, router: Router) {
    if (authService.isLoggedOut()) {
      router.navigateByUrl("/");
    }
  }
}
