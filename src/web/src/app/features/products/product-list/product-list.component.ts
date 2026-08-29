import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../core/services/product.service';

interface Product {
  productId: number;
  name: string;
  description: string;
  category: string;
  price: number;
  isAvailable: boolean;
}

@Component({
  standalone: false,
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private productService: ProductService) {}

  // ngOnInit runs when the component loads
  // This is like useEffect(() => {}, []) in React
  ngOnInit(): void {
    this.productService.getAll().subscribe({
      next: (data) => {
        this.products = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load products. Please try again.';
        this.isLoading = false;
      }
    });
  }
}