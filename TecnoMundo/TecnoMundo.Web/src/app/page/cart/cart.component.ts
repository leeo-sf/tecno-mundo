import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Cart } from '../../../interface/Cart';
import localPt from '@angular/common/locales/pt';
import { CommonModule, NgFor, NgIf, registerLocaleData } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { CartDetails } from '../../../interface/CartDetails';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../../template/dialog/dialog.component';
import { CartService } from '../../service/cart.service';
import { ApplyCouponComponent } from '../../template/apply-coupon/apply-coupon.component';

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
    ApplyCouponComponent
  ],
  providers: [ NgFor ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  private readonly dialog = inject(MatDialog);
  public cart!: Cart;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.cart = data["cart"];
    });

    console.log(this.subTotal);
  }

  get subTotal(): number {
    return this.cart.cartDetails.reduce((x, item) => {
      return x + (item.product.price * item.count);
    }, 0);
  }

  updateCart(cartDetail: CartDetails): void {
    console.log("update");
  }

  deleteCart(idCartDetails: number): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '300px'
    });

    const token: string = localStorage.getItem("token") ?? "";

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'confirm') {
        this.cartService.serviceClearCart(idCartDetails, JSON.parse(token)).subscribe((result) => {
          if (result) {
            this.router.navigate(['/my-cart']);
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
        this.cartService.serviceClearCart(JSON.parse(userId), JSON.parse(token)).subscribe((result) => {
          if (result) {
            this.router.navigate(['/my-cart']);
          }
        });
      }
    });
  }

}
