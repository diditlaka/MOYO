import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = 'https://moyo-ftg6h6cabjczexf7.southafricanorth-01.azurewebsites.net/api/orders';

  constructor(private http: HttpClient) {}

  getMyOrders(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  createOrder(productId: number, quantity: number): Observable<any> {
    return this.http.post<any>(this.apiUrl, { productId, quantity });
  }
}