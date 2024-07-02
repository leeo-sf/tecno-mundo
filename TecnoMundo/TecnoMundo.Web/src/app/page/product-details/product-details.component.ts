import { Component, OnInit } from '@angular/core';
import { Product } from '../../../interface/Product';
import { ActivatedRoute } from '@angular/router';
import { AddToCartComponent } from '../../template/add-to-cart/add-to-cart.component';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    AddToCartComponent
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.css'
})
export class ProductDetailsComponent implements OnInit {
  product!: Product;

  constructor(
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.product = data['product'];
    });
  }
}
