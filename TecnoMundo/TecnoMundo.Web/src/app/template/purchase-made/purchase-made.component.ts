import { Component, OnInit } from '@angular/core';
import { OrderMade } from '../../../interface/OrderMade';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-purchase-made',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    NgIf
  ],
  providers: [ NgFor ],
  templateUrl: './purchase-made.component.html',
  styleUrl: './purchase-made.component.css'
})
export class PurchaseMadeComponent implements OnInit {
  public orderMade!: OrderMade;
  
  ngOnInit(): void {
    this.orderMade = history.state.orderMade;
  }

}
