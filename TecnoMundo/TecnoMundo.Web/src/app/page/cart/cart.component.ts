import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Cart } from '../../../interface/Cart';
import localPt from '@angular/common/locales/pt';
import { CommonModule, NgFor, registerLocaleData } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { CartDetails } from '../../../interface/CartDetails';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../../template/dialog/dialog.component';
import { CartService } from '../../service/cart.service';

registerLocaleData(localPt)

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    RouterLink
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
        this.cartService.serviceRemoveFromCart(idCartDetails, JSON.parse(token)).subscribe((result) => {
          if (result) {
            this.router.navigate(['/my-cart']);
          }
        });
      }
    });
  }

}
