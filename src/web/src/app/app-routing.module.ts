import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { ProductListComponent } from './features/products/product-list/product-list.component';
import { OrderListComponent } from './features/orders/order-list/order-list.component';
import { PlaceOrderComponent } from './features/orders/place-order/place-order.component';
import { authGuard } from './core/guards/auth.guard';
import { HomeComponent } from './features/home/home.component';

// Routes define which component to show for each URL
// This is the same concept as React Router
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  // canActivate: [authGuard] means you must be logged in to visit these pages
  // If not logged in, the guard redirects to /login
  { path: 'products', component: ProductListComponent, canActivate: [authGuard] },
  { path: 'orders', component: OrderListComponent, canActivate: [authGuard] },
  { path: 'orders/new', component: PlaceOrderComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: 'products' } // Catch-all for unknown URLs
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }