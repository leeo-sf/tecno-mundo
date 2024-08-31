import { Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { OrderService } from '../../service/order.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderHeader } from '../../../interface/OrderHeader';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ProductService } from '../../service/product.service';
import { Product } from '../../../interface/Product';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    NgIf,
    RouterLink
  ],
  providers: [
    NgFor,
    ProductService
  ],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent implements OnInit {
  @ViewChildren('orders') ordersDivs!: QueryList<ElementRef>;
  @ViewChildren('buttons') buttonsDiv!: QueryList<ElementRef>;
  public orders!: OrderHeader[];

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.orders = data["orders"];
    });

    this.orders.forEach((orderHeader) => {
      orderHeader.orderDetails.forEach((orderDetail) => {
        this.productService.serviceGetProductById(orderDetail.productId.toString()).subscribe((result: Product) => {
          orderDetail.product = result;
        });
      });
    });
  }

  buyAgain(orderHeader: OrderHeader): void {
    console.log(orderHeader);
  }

  showHideOrder(orderId: number): void {
    this.buttonsDiv.forEach((div) => {
      const id = div.nativeElement.getAttribute("data-button-id");
        if (id == orderId && div.nativeElement.style.transform === 'rotate(90deg)') {
          div.nativeElement.style.transform = "rotate(-90deg)";
        }
        else {
          div.nativeElement.style.transform = "rotate(90deg)"
        }
    });

    this.ordersDivs.forEach((div) => {
      const id = div.nativeElement.getAttribute("data-order-id");
      if (id == orderId && div.nativeElement.style.display === 'none') {
        div.nativeElement.style.display = "block";
      }
      else {
        div.nativeElement.style.display = "none"
      }
    });
  }

}
