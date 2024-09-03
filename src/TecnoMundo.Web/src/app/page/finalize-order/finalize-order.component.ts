import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Coupon } from '../../../interface/Coupon';
import { FinalizeOrder } from '../../../interface/FinalizeOrder';
import { CommonModule, NgIf } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Checkout } from '../../../interface/Checkout';
import { CartService } from '../../service/cart.service';
import { OrderMade } from '../../../interface/OrderMade';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-finalize-order',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    CommonModule
  ],
  templateUrl: './finalize-order.component.html',
  styleUrl: './finalize-order.component.css'
})
export class FinalizeOrderComponent implements OnInit {
  public orderData!: FinalizeOrder;
  public couponApplied!: Coupon;
  public finalizeOrderForm!: FormGroup;

  public firstName: string = "NOAH";
  public lastName: string = "JACOB";
  public expirationMonth: string = "02";
  public expirationYear: string = "30";
  public cvv: string = "123";
  public cardNumber: string = "1234 5678 9101 1121";

  constructor(
    private _snackBar: MatSnackBar,
    private cartService: CartService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.orderData = history.state.finalizeOrder;

    this.finalizeOrderForm = new FormGroup({
      firstName: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(20)
      ]),
      lastName: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(40)
      ]),
      phone: new FormControl('', [
        Validators.required,
        Validators.minLength(11),
        Validators.maxLength(15)
      ]),
      email: new FormControl('', [
        Validators.required,
        Validators.email
      ]),
      cardNumber: new FormControl('', [
        Validators.required,
        Validators.minLength(16),
        Validators.maxLength(19)
      ]),
      cvv: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(3)
      ]),
      cardExpirationMonth: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(2)
      ]),
      cardExpirationYear: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(2)
      ])
    });
  }

  get subTotal(): number {
    return this.orderData.cart.cartDetails.reduce((x, item) => {
      return x + (item.product.price * item.count);
    }, 0);
  }

  get subTotalWithDiscount(): number {
    const subTotal = this.subTotal;
    const subTotalWithDiscount = this.orderData?.coupon ? 
      subTotal - this.orderData.coupon?.discountAmount : subTotal;

    return subTotalWithDiscount;
  }

  get totalItems(): number {
    return this.orderData.cart.cartDetails.reduce((x, item) => {
      return x + (item.count);
    }, 0);
  }

  inptCardNumber(event: any): void {
    if (event.target.value.length === 0) {
      this.cardNumber = "1234 5678 9101 1121"
      return;
    }

    if (event.target.value.length > 19) {
      this.finalizeOrderForm.get("cardNumber")?.setValue(event.target.value.slice(0, -1));
      return;
    }

    const input = event.target.value.replace(/\D/g, '');
    this.cardNumber = this.applyMask(input);

    this.finalizeOrderForm.get("cardNumber")?.setValue(input.replace(/(\d{4})(?=\d)/g, '$1 '));
  }

  inptFirstname(event: any): void {
    if (event.target.value.length === 0) {
      this.firstName = "NOAH"
      return;
    }
    const input = event.target.value.toUpperCase();
    this.firstName = input;
  }

  inptLastname(event: any): void {
    if (event.target.value.length === 0) {
      this.lastName = "NOAH"
      return;
    }
    const input = event.target.value.toUpperCase();
    this.lastName = input;
  }

  inptCvv(event: any): void {
    if (event.target.value.length === 0) {
      this.cvv = "123"
      return;
    }
    const input = event.target.value.replace(/\D/g, '');
    this.cvv = input;
  }

  inptExpirationMonth(event: any): void {
    if (event.target.value.length === 0) {
      this.expirationMonth = "02"
      return;
    }

    const input = event.target.value.replace(/\D/g, '');
    this.expirationMonth = input;
  }

  inptExpirationYear(event: any): void {
    if (event.target.value.length === 0) {
      this.expirationYear = "30"
      return;
    }

    const input = event.target.value.replace(/\D/g, '');
    this.expirationYear = input;
  }

  private formatCardNumber(): string {
    const cardNumberInput: string = this.finalizeOrderForm.get("cardNumber")?.value;

    const firstPart = cardNumberInput.slice(0,4);
    const maskedPart1 = cardNumberInput.slice(4,8);
    const maskedPart2 = cardNumberInput.slice(8,12);
    const lastPart = cardNumberInput.slice(12,16);

    return `${firstPart} ${maskedPart1} ${maskedPart2} ${lastPart}`;
  }

  private applyMask(cardNumberInput: string): string {
    if (cardNumberInput.length <= 4) {
      return cardNumberInput; 
    }

    const firstPart = cardNumberInput.slice(0,4);
    const maskedPart1 = cardNumberInput.slice(4,8).replace(/./g, '*');
    const maskedPart2 = cardNumberInput.slice(8,12).replace(/./g, '*');
    const lastPart = cardNumberInput.slice(12,16);

    return `${firstPart} ${maskedPart1} ${maskedPart2} ${lastPart}`;
  }

  closeOrder(): void {
    if (this.finalizeOrderForm.invalid) {
      this._snackBar.open("Dados inválidos", "close", { duration: 3 * 1000 });
      return;
    }

    const currentDate = new Date();
    const token: string = JSON.parse(localStorage.getItem("token") ?? "");
    const userId: string = JSON.parse(localStorage.getItem("user-id") ?? "");
    const firstName: string = this.finalizeOrderForm.get("firstName")?.value.toUpperCase() ?? "";
    const lastName: string = this.finalizeOrderForm.get("lastName")?.value.toUpperCase() ?? "";
    const phone: string = this.finalizeOrderForm.get("phone")?.value ?? "";
    const email: string = this.finalizeOrderForm.get("email")?.value ?? "";
    const cardNumber: string = this.finalizeOrderForm.get("cardNumber")?.value ?? "";
    const expirateMonth: string = this.finalizeOrderForm.get("cardExpirationMonth")?.value ?? "";
    const expirateYear: string = this.finalizeOrderForm.get("cardExpirationYear")?.value ?? "";
    const cvv: string = this.finalizeOrderForm.get("cvv")?.value ?? "";
    
    var checkout: Checkout = {
      messageCreated: currentDate.toISOString(),
      userId: userId,
      couponCode: this.orderData.coupon?.couponCode ?? "",
      purchaseAmount: this.subTotal,
      discountAmount: this.orderData.coupon?.discountAmount ?? 0,
      fistrName: firstName,
      lastName: lastName,
      dateTime: currentDate.toISOString(),
      phone: phone,
      email: email,
      cardNumber: cardNumber,
      cvv: cvv,
      expireMonthYear: `${expirateMonth}/${expirateYear}`,
      cartTotalItems: this.totalItems,
      cartDetails: this.orderData.cart.cartDetails
    }

    this.cartService.serviceCheckout(checkout, token).subscribe((result) => {
      const orderResult: Checkout = result;

      let orderMade: OrderMade = {
        cartDetails: orderResult.cartDetails,
        finalCardPurchased: Number.parseInt(orderResult.cardNumber.slice(-4)),
        totalItems: this.totalItems,
        purchaseAmount: orderResult.purchaseAmount,
        discountAmount: orderResult.discountAmount
      };
      this.router.navigate(['finalize-order/order'], { state: { orderMade: orderMade } });
    }, (error) => {
      console.log(error);
    });
  }
}
