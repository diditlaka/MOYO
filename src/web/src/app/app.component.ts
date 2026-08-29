import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AuthService } from './core/services/auth.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  title = 'web';
  showSidebar = false;

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.updateSidebar();

    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(() => {
        this.updateSidebar();
      });

    this.authService.isLoggedIn$.subscribe(() => {
      this.updateSidebar();
    });
  }

  private updateSidebar(): void {
    const publicRoutes = ['/', '/login', '/register'];
    const currentUrl = this.router.url.split('?')[0];

    this.showSidebar =
      !!this.authService.getToken() &&
      !publicRoutes.includes(currentUrl);
  }
}