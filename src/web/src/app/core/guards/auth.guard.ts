import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

// This guard protects routes that require login
// If the user has no token, they get redirected to /login
// This is the same as a PrivateRoute in React Router
export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.getToken()) {
    return true; // Allow access
  }

  router.navigate(['/login']);
  return false; // Block access
};