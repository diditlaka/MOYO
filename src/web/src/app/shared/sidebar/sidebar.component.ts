import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {
  userName = '';
  userEmail = '';
  profileOpen = false;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    const user = this.authService.getUser();

    if (user) {
      this.userName = user.name;
      this.userEmail = user.email;
    }
  }

  toggleProfile(): void {
    this.profileOpen = !this.profileOpen;
  }

  logout(): void {
    this.authService.logout();
  }
}
