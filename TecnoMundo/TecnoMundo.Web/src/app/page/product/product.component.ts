import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../service/product.service';
import { Product } from '../../../interface/Product';
import { NgFor } from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import { Category } from '../../../interface/Category';
import { ActivatedRoute, Router } from '@angular/router';
import { map } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [
    NgFor,
    MatIconModule,
    FormsModule
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
  public productName!: string;

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

  filterProductsByName(): void {
    this.productService.serviceGetProductsByName(this.productName).subscribe((data => {
      this.listOfProducts = data;
    }))
  }
}
