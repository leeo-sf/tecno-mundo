import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../service/product.service';
import { Product } from '../../../interface/Product';
import { NgFor } from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import { SubHeaderComponent } from '../../template/sub-header/sub-header.component';
import { Category } from '../../../interface/Category';
import { ActivatedRoute } from '@angular/router';
import { productResolve } from '../../service/_guard/productResolve';

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

  constructor(
    private serviceProduct: ProductService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.listOfProducts = data['products'];
      this.listOfCategories = data['categories'];
    });
  }

  filterProductsByCategory(idCategory: number): void {
    console.log(idCategory);
  }
}
