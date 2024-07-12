import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Cart } from '../../../interface/Cart';
import localPt from '@angular/common/locales/pt';
import { CommonModule, NgFor, registerLocaleData } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

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
  public cart!: Cart;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.cart = data["cart"];
    });
  }

  teste(): void {
    console.log("click");
  }

}
