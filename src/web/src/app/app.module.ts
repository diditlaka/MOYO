import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Shared
import { NavbarComponent } from './shared/navbar/navbar.component';


// Auth
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';

// Products
import { ProductListComponent } from './features/products/product-list/product-list.component';

// Orders
import { OrderListComponent } from './features/orders/order-list/order-list.component';
import { PlaceOrderComponent } from './features/orders/place-order/place-order.component';

// Interceptor — adds JWT token to every request automatically
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { HomeComponent } from './features/home/home.component';
import { SidebarComponent } from './shared/sidebar/sidebar.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    RegisterComponent,
    ProductListComponent,
    OrderListComponent,
    PlaceOrderComponent,
    HomeComponent,
    SidebarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,    // Lets us make HTTP calls to our API
    FormsModule,         // Lets us use template-driven forms
    ReactiveFormsModule  // Lets us use reactive forms
  ],
  providers: [
    {
      // Register our interceptor so it runs on every HTTP request
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }