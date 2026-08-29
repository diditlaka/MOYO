import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  standalone: false,
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  // These variables are bound to the form inputs
  // When the user types, these update automatically
  // This is like useState in React
  email = '';
  password = '';
  errorMessage = '';
  isLoading = false;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login({ email: this.email, password: this.password })
      .subscribe({
        next: () => {
          // Login successful — navigate to products page
          this.router.navigate(['/products']);
        },
        error: () => {
          this.errorMessage = 'Invalid email or password. Please try again.';
          this.isLoading = false;
        }
      });
  }
}