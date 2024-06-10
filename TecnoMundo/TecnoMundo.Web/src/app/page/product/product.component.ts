import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../service/product.service';
import { Product } from '../../../interface/Product';
import { NgFor } from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import { SubHeaderComponent } from '../../template/sub-header/sub-header.component';
import { Category } from '../../../interface/Category';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [
    SubHeaderComponent,
    NgFor,
    MatIconModule
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

  constructor(private serviceProduct: ProductService) {}

  ngOnInit(): void {
    this.getAllProducts();
    this.getAllCategories();
  }

  getAllProducts(): void {
    this.serviceProduct.serviceListProducts().subscribe((products: Product[]) => {
      this.listOfProducts = products;
    });
  }

  getAllCategories(): void {
    this.serviceProduct.serviceListProductCategories().subscribe((categories: Category[]) => {
      console.log(categories);
      this.listOfCategories = categories;
    });
  }

  filterProductsByCategory(idCategory: number): void {
    console.log(idCategory);
  }
}
