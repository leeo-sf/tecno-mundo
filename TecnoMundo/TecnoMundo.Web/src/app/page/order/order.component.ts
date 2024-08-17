import { Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { OrderService } from '../../service/order.service';
import { ActivatedRoute } from '@angular/router';
import { OrderHeader } from '../../../interface/OrderHeader';
import { CommonModule, NgFor } from '@angular/common';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [
    CommonModule
  ],
  providers: [
    NgFor
  ],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent implements OnInit {
  @ViewChildren('orders') ordersDivs!: QueryList<ElementRef>;
  public orders!: OrderHeader[];

  constructor(
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.orders = data["orders"];
    });
  }

  showHideOrder(orderId: number): void {
    this.ordersDivs.forEach((div) => {
      const id = div.nativeElement.getAttribute("data-order-id");
      if (id == orderId && div.nativeElement.style.display === 'none') {
        div.nativeElement.style.display = "block";
        div.nativeElement.style.transition = "2.0s ease-in";
      }
      else {
        div.nativeElement.style.display = "none"
      }
    });
  }

}
