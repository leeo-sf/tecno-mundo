import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router, RouterLink } from '@angular/router';
import { Cart } from '../../../interface/Cart';
import localPt from '@angular/common/locales/pt';
import { CommonModule, NgFor, NgIf, registerLocaleData } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { CartDetails } from '../../../interface/CartDetails';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../../template/dialog/dialog.component';
import { CartService } from '../../service/cart.service';
import { ApplyCouponComponent } from '../../template/apply-coupon/apply-coupon.component';
import { Coupon } from '../../../interface/Coupon';
import { FormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FinalizeOrder } from '../../../interface/FinalizeOrder';

registerLocaleData(localPt)

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    RouterLink,
    NgIf,
    MatIconModule,
    ApplyCouponComponent,
    FormsModule
  ],
  providers: [ NgFor ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  private readonly dialog = inject(MatDialog);
  public cart!: Cart;
  public appliedCoupon!: Coupon;
  public appliedCoupon$!: boolean;
  public cartIsEmpty: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cartService: CartService,
    private _snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.popularCart();
  }

  get subtotalWithDiscountApplied(): number {
    const subtotal = this.cart.cartHeader.purchaseAmount ?? 0;

    return subtotal - this.appliedCoupon.discountAmount;
  }

  updateCart(cartDetail: CartDetails): void {
    const token: string = JSON.parse(localStorage.getItem("token") ?? "");

    let cart: Cart = {
      cartHeader: this.cart.cartHeader,
      cartDetails: [cartDetail]
    }
    
    this.cartService.serviceUpdateToCart(cart, token).subscribe((result) => {
      if (result !== undefined) {
        const details = this.cart.cartDetails;
        this.cart.cartDetails = []
        this.cart.cartDetails = details.filter(cartDetail => result.cartDetails.map(item => item.id == cartDetail.id));
        this.cartValue();
      }
    }, (error) => {
      this._snackBar.open("Unable to update quantity", "Close", {
        horizontalPosition: "end",
        verticalPosition: "top",
        duration: 3 * 1000
      });
    });
  }

  deleteCart(idCartDetails: number): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '300px'
    });

    const token: string = localStorage.getItem("token") ?? "";

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'confirm') {
        this.cartService.serviceRemoveFromCart(idCartDetails, JSON.parse(token)).subscribe((result) => {
          if (result) {
            this.cart.cartDetails = this.cart.cartDetails.filter(cart => cart.id !== idCartDetails);
            this.validateCountDetails(this.cart.cartDetails);
          }
        });
      }
    });
  }

  clearCart(): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '300px'
    });

    const userId: string = localStorage.getItem("user-id") ?? "";
    const token: string = localStorage.getItem("token") ?? "";

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'confirm') {
        this.clearConfirm(userId, token);
      }
    });
  }

  private clearConfirm(userId: string, token: string): void {
    this.cartService.serviceClearCart(JSON.parse(userId), JSON.parse(token)).subscribe((result) => {
      if (result) {
        this.cartIsEmpty = true;
      }
      else {
        this.cartIsEmpty = false;
      }
    });
  }

  onCouponApplied(coupon: Coupon) {
    this.appliedCoupon$ = !(!coupon || Object.values(coupon).every(value => value === null || value === undefined));
    this.appliedCoupon = coupon;
  }

  finalizeOrder(): void {
    const checkout: FinalizeOrder = {
      cart: this.cart,
      coupon: this.appliedCoupon
    }
    this.router.navigate(['finalize-order'], { state: { finalizeOrder: checkout } });
  }

  private validateCountDetails(cartDetails: CartDetails[]) {
    this.cartIsEmpty = cartDetails.length === 0 ? true : false;
    this.cartValue();
  }

  private cartValue(): void {
    this.cart.cartHeader.purchaseAmount = this.cart.cartDetails.reduce((x, item) => {
      return x + (item.product.price * item.count);
    }, 0);
  }

  private popularCart(): void {
    this.route.data.subscribe((data) => {
      this.cart = data["cart"];
      this.validateCountDetails(this.cart.cartDetails);
      this.cartValue();
    });
  }

}
