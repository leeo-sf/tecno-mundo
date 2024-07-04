import { Component, Input } from '@angular/core';
import { Product } from '../../../interface/Product';
import { AuthService } from '../../service/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CartService } from '../../service/cart.service';
import { Cart } from '../../../interface/Cart';
import { CartHeader } from '../../../interface/CartHeader';
import { CartDetails } from '../../../interface/CartDetails';

@Component({
  selector: 'app-add-to-cart',
  standalone: true,
  imports: [],
  providers: [
    CartService
  ],
  templateUrl: './add-to-cart.component.html',
  styleUrl: './add-to-cart.component.css'
})
export class AddToCartComponent {
  @Input() product!: Product;
  @Input() amount!: number;
  private cart!: Cart;

  constructor(
    private authService: AuthService,
    private _snackBar: MatSnackBar,
    private cartService: CartService
  ) {}

  addToCart(): void {
    this.authService.isLoggedIn$.subscribe((data) => {
      if (!data) {
        this._snackBar.open("Access your account", "Close", {
          horizontalPosition: "end",
          verticalPosition: "top",
          duration: 3 * 1000
        })
      }
      else {
        console.log("Add to cart");
        const cart: Cart = this.setCart(this.product, this.amount);

        console.log(JSON.stringify(cart));
      }
    })
  }

  private setCart(product: Product, count: number): Cart {
    let userId: string = localStorage.getItem("user-id") ?? "";
    let cartHeader: CartHeader = {
      id: 0,
      userId: JSON.parse(userId),
      couponCode: ""
    };
    let cartDetails: CartDetails = {
      id: 0,
      cartHeaderId: 0,
      cartHeader: cartHeader,
      productId: this.product.id,
      product: this.product,
      count: this.amount
    };
    const cart: Cart = { cartHeader: cartHeader, cartDetails: [cartDetails] };

    return cart;
  }
}
