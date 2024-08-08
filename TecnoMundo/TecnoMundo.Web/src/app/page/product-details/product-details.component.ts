import { Component, OnInit } from '@angular/core';
import { Product } from '../../../interface/Product';
import { ActivatedRoute } from '@angular/router';
import { AddToCartComponent } from '../../template/add-to-cart/add-to-cart.component';
import { FormsModule } from '@angular/forms';
import localPt from '@angular/common/locales/pt';
import { CommonModule, registerLocaleData } from '@angular/common';

registerLocaleData(localPt)

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    AddToCartComponent,
    FormsModule,
    CommonModule
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.css'
})
export class ProductDetailsComponent implements OnInit {
  product!: Product;
  amount: number = 1;

  constructor(
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.product = data['product'];
    });
  }
}
