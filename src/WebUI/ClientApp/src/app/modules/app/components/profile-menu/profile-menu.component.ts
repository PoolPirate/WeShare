import { Component, ViewChild } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';
import { AuthService } from '../../../../services/authservice';

@Component({
  selector: 'app-profile-menu',
  templateUrl: './profile-menu.component.html'
})
export class ProfileMenuComponent {
  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;

  constructor(private authService: AuthService) {
  }

  get displayName() {
    return this.authService.getDisplayName()!;
  }

  get username() {
    return this.authService.getUsername()!;
  }
}
