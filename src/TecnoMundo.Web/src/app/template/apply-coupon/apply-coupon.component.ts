import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { CartService } from '../../service/cart.service';
import { Cart } from '../../../interface/Cart';
import { Router } from '@angular/router';
import { response } from 'express';
import { Coupon } from '../../../interface/Coupon';
import { CommonModule, NgIf } from '@angular/common';
import { CouponService } from '../../service/coupon.service';

@Component({
  selector: 'app-apply-coupon',
  standalone: true,
  imports: [
    NgIf,
    CommonModule
  ],
  templateUrl: './apply-coupon.component.html',
  styleUrl: './apply-coupon.component.css'
})
export class ApplyCouponComponent implements OnInit {
  @Input() cart!: Cart;
  @Output() couponApplied: EventEmitter<Coupon> = new EventEmitter();
  public coupon!: Coupon;
  public couponApplied$!: boolean;
  public messageAttemptToApplyCoupon: string = "";

  ngOnInit(): void {
    if (this.cart.cartHeader.couponCode) {
      const token: string = JSON.parse(localStorage.getItem("token") ?? "");
      this.getCoupon(this.cart.cartHeader.couponCode, token);
    }

    this.couponApplied$ = false;
  }

  constructor(
    private cartService: CartService,
    private router: Router,
    private couponService: CouponService
  ) {}

  private getCoupon(couponCode: string, token: string): void {
    this.couponService.serviceGetCouponCode(couponCode, token).subscribe((result) => {
      if (result) {
        this.couponApplied$ = true;
        this.coupon = result;
        this.couponApplied.emit(this.coupon);
      }
    });
  }

  applyCoupon(couponCode: string) {
    if (couponCode === "") {
      this.messageAttemptToApplyCoupon = "Insert Coupon";
      setTimeout(() => {
        this.messageAttemptToApplyCoupon = "";
      }, 3000);
      return;
    }

    const userId: string = this.cart.cartHeader.userId;
    const token: string = JSON.parse(localStorage.getItem("token") ?? "");
    
    this.cartService.serviceApplyCoupon(couponCode, userId, token).subscribe((response) => {
      this.couponApplied$ = true;
      this.getCoupon(couponCode, token)
    }, 
    (error) => {
        let message = "Invalid Coupon";
        this.messageAttemptToApplyCoupon = message;
        setTimeout(() => {
          this.messageAttemptToApplyCoupon = "";
        }, 3000);
      }
    );
  }

  removeCoupon(): void {
    const userId: string = this.cart.cartHeader.userId;
    const token: string = JSON.parse(localStorage.getItem("token") ?? "");

    this.cartService.serviceRemoveCoupon(userId, token).subscribe((result) => {
      this.coupon = new Coupon();
      this.couponApplied$ = false;
      this.couponApplied.emit(this.coupon);
    });
  }
}
