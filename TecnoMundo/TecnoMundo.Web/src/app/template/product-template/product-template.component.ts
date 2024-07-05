import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ProductService } from '../../service/product.service';
import { Product } from '../../../interface/Product';
import { NgFor, NgIf } from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import { Category } from '../../../interface/Category';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AddToCartComponent } from '../add-to-cart/add-to-cart.component';

@Component({
  selector: 'app-product-template',
  standalone: true,
  imports: [
    NgFor,
    MatIconModule,
    FormsModule,
    NgIf,
    AddToCartComponent
  ],
  providers: [
    NgFor,
    ProductService
  ],
  templateUrl: './product-template.component.html',
  styleUrl: './product-template.component.css'
})
export class ProductTemplateComponent implements OnInit {
  listOfProducts!: Product[];
  public msgProductNotFound!: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      if (data['products'].error) {
        this.msgProductNotFound = data['products'].error;
        this.listOfProducts = [];
      } else {
        this.listOfProducts = data['products'];
      }
    }, (error) => {
      this.msgProductNotFound = error;
      this.listOfProducts = []
    });
  }
}
