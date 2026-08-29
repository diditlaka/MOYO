import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';

@Component({
  standalone: false,
  selector: 'app-place-order',
  templateUrl: './place-order.component.html',
  styleUrl: './place-order.component.css'
})
export class PlaceOrderComponent implements OnInit {
  productId: number = 0;
  productName: string = '';
  price: number = 0;
  quantity: number = 1;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private router: Router
  ) {}

  ngOnInit(): void {
  this.route.queryParams.subscribe(params => {
    this.productId = Number(params['productId']);
    this.productName = params['productName'] || '';
    this.price = Number(params['price']);

    if (!this.productId || !this.productName || !this.price) {
      this.router.navigate(['/products']);
    }
  });
}

  onSubmit(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.orderService.createOrder(this.productId, this.quantity).subscribe({
      next: () => {
        this.successMessage = 'Order placed successfully!';
        setTimeout(() => this.router.navigate(['/orders']), 2000);
      },
      error: () => {
        this.errorMessage = 'Failed to place order. Please try again.';
        this.isLoading = false;
      }
    });
  }

  get total(): number {
    return this.price * this.quantity;
  }
}