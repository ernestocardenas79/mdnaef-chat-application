import { Component } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  autheticatedUser$ = this.authService.userAuthenticated$;
  constructor(
    private authService: AuthenticationService,
    private router: Router
  ) {}

  logout() {
    localStorage.removeItem('token');
    this.authService.logoutUser();
    this.router.navigate(['/login']);
  }
}
