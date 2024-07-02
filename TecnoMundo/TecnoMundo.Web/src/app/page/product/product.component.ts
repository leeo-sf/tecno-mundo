import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../service/product.service';
import { Product } from '../../../interface/Product';
import { NgFor, NgIf } from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import { Category } from '../../../interface/Category';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AddToCartComponent } from '../../template/add-to-cart/add-to-cart.component';

@Component({
  selector: 'app-product',
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
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit {
  listOfProducts!: Product[];
  listOfCategories!: Category[];
  public msgProductNotFound!: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.listOfProducts = data['products'];
      this.listOfCategories = data['categories'];
    });
  }

  filterProductsByCategory(idCategory: number): void {
    this.productService.serviceGetProductsByCategoryId(idCategory).subscribe((data => {
      this.listOfProducts = data;
    }));
  }

  filterProductsByName(productName: string): void {
    this.productService.serviceGetProductsByName(productName).subscribe((data => {
      if (data.length == 0) {
        this.msgProductNotFound = productName;
      }

      this.listOfProducts = data;
    }));
  }
}
