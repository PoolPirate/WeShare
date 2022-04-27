import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/authservice';

@Component({
  selector: 'app-logout-component',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent {
  constructor(private authService: AuthService, private router: Router) {

    if (authService.isLoggedOut()) {
      router.navigateByUrl("/");
    }

    authService.logout();
    router.navigateByUrl("/login");
  }
}
