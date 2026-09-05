import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

// These match the request/response shapes from our C# API
interface LoginRequest {
  email: string;
  password: string;
}

interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

interface AuthResponse {
  token: string;
  name: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // The URL of our C# API
  private apiUrl = 'https://moyo-ftg6h6cabjczexf7.southafricanorth-01.azurewebsites.net/api/auth';

  // BehaviorSubject tracks whether the user is logged in
  // Components can subscribe to this to react when login state changes
  // This is similar to React Context
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

  isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, data).pipe(
      tap(response => this.saveToken(response))
    );
  }

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, data).pipe(
      // tap runs a side effect without changing the response
      // here we save the token after a successful login
      tap(response => this.saveToken(response))
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }
isLoggedIn(): boolean {
  return this.loggedIn.value;
}
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUser(): { name: string; email: string } | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  private saveToken(response: AuthResponse): void {
    localStorage.setItem('token', response.token);
    localStorage.setItem('user', JSON.stringify({
      name: response.name,
      email: response.email
    }));
    this.loggedIn.next(true);
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('token');
  }
}