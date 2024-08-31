import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Product } from '../../../interface/Product';
import { ActivatedRoute } from '@angular/router';
import { AddToCartComponent } from '../../template/add-to-cart/add-to-cart.component';
import { FormsModule } from '@angular/forms';
import localPt from '@angular/common/locales/pt';
import { CommonModule, NgFor, registerLocaleData } from '@angular/common';
import { InstallmentOptions } from '../../../interface/InstallmentOptions';
import { MatIconModule } from '@angular/material/icon';

registerLocaleData(localPt)

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    AddToCartComponent,
    FormsModule,
    CommonModule,
    MatIconModule
  ],
  providers: [
    NgFor
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.css'
})
export class ProductDetailsComponent implements OnInit {
  product!: Product;
  amount: number = 1;
  installments: InstallmentOptions[] = [];
  @ViewChild('contentInstallments') hideOrShowInstallments!: ElementRef;
  @ViewChild('btnSeeInstallments') btnInstallments!: ElementRef;

  constructor(
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.product = data['product'];
    });
    this.getInstallmentys();
  }

  public getInstallmentys(): void {
    for (let x = 1; x <= 12; x++) {
      this.installments.push({
        numberOfInstallments: x,
        installmentValue: this.product.price / x
      });
    }
  }

  public expandInstallments(event: any): void {
    let divElement = this.hideOrShowInstallments.nativeElement;
    let divElementBtn = this.btnInstallments.nativeElement;
    
    if (divElement.style.display == 'none') {
      divElement.style.display = "flex";
      divElementBtn.style.transform = "rotate(90deg)";
    }
    else {
      divElement.style.display = "none";
      divElementBtn.style.transform = "rotate(0deg)";
    }
  }
}
