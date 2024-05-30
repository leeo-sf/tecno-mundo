import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../service/product.service';
import { Product } from '../../../interface/Product';
import { NgFor } from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import { SubHeaderComponent } from '../../template/sub-header/sub-header.component';

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

  constructor(private serviceProduct: ProductService) {}

  ngOnInit(): void {
    this.getAllProducts();
  }

  getAllProducts(): void {
    this.serviceProduct.serviceListProducts().subscribe((products: Product[]) => {
      this.listOfProducts = products;
    })
  }
}
