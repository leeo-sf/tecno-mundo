import { Component, Input } from '@angular/core';
import { Product } from '../../../interface/Product';

@Component({
  selector: 'app-add-to-cart',
  standalone: true,
  imports: [],
  templateUrl: './add-to-cart.component.html',
  styleUrl: './add-to-cart.component.css'
})
export class AddToCartComponent {
  @Input() product!: Product;
  @Input() amount!: number;

  addToCart(): void {
    console.log("Add to cart");
    console.log(this.product);
    console.log(this.amount);
  }
}
