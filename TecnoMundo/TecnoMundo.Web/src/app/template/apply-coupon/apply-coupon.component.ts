import { Component, Input } from '@angular/core';
import { CartService } from '../../service/cart.service';
import { Cart } from '../../../interface/Cart';
import { Router } from '@angular/router';
import { response } from 'express';
import { Coupon } from '../../../interface/Coupon';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-apply-coupon',
  standalone: true,
  imports: [
    NgIf
  ],
  templateUrl: './apply-coupon.component.html',
  styleUrl: './apply-coupon.component.css'
})
export class ApplyCouponComponent {
  @Input() cart!: Cart;
  public coupon!: Coupon;
  public couponApplied!: boolean;

  constructor(
    private cartService: CartService,
    private router: Router
  ) {}

  applyCoupon(couponCode: string) {
    const userId: string = this.cart.cartHeader.userId;
    const token: string = localStorage.getItem("token") ?? "";

    this.cartService.serviceApplyCoupon(couponCode.toUpperCase(), userId, JSON.parse(token)).subscribe((response) => {
      console.log("entrou no subscribe");
      console.log(response);
    })
  }
}
