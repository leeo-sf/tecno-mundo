import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../../interface/Product';
import { CommonModule, NgFor, NgIf, registerLocaleData } from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { AddToCartComponent } from '../add-to-cart/add-to-cart.component';
import localPt from '@angular/common/locales/pt';
import { RouterLink } from '@angular/router';

registerLocaleData(localPt)

@Component({
  selector: 'app-product-template',
  standalone: true,
  imports: [
    NgFor,
    MatIconModule,
    FormsModule,
    NgIf,
    AddToCartComponent,
    CommonModule,
    RouterLink
  ],
  providers: [
    NgFor
  ],
  templateUrl: './product-template.component.html',
  styleUrl: './product-template.component.css'
})
export class ProductTemplateComponent implements OnInit {
  @Input() msgProductNotFound!: string;
  @Input() listOfProducts!: Product[];

  constructor() {}

  ngOnInit(): void {}
  
}
