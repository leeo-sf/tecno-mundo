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

  constructor(
    private authService: AuthService,
    private _snackBar: MatSnackBar,
    private cartService: CartService
  ) {  }

  addToCart(): void {
    if (!this.authService.loggedInUser) {
      const message = "Access your account";
      this.requestedOperationMessage(message);
    }
    else {
      const token = JSON.parse(localStorage.getItem("token") ?? "");
      const cart: Cart = this.setCart(this.product, this.amount);

      this.cartService.serviceAddItemToCart(cart, token).subscribe((data) => {
        const message = "Product added to cart";
        this.requestedOperationMessage(message);
      }, (error) => {
        console.log(error);
      });
    }
  }

  private setCart(product: Product, count: number): Cart {
    let userId = localStorage.getItem("user-id") ?? "";

    let cartHeader: CartHeader = {
      userId: JSON.parse(userId),
      couponCode: ""
    };
    
    let cartDetail: CartDetails = {
      count: count,
      productId: product.id,
      product: product
      //cartHeader: { id: 0, userId: "", couponCode: "" }
    };

    let cartDetails: CartDetails[] = [];
    cartDetails.push(cartDetail);

    let cart: Cart = {
      cartHeader: cartHeader,
      cartDetails: cartDetails
    }

    return cart;
  }

  private requestedOperationMessage(message: string): void {
    this._snackBar.open(message, "Close", {
      horizontalPosition: "end",
      verticalPosition: "top",
      duration: 3 * 1000
    });
  }
}
