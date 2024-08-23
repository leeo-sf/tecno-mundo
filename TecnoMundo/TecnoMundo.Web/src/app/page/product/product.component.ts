import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { ProductTemplateComponent } from '../../template/product-template/product-template.component';
import { MatSliderModule } from '@angular/material/slider'
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from '../../../interface/Category';
import { NgFor, NgIf } from '@angular/common';
import { MatPaginatorModule } from '@angular/material/paginator';
import { Product } from '../../../interface/Product';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [
    NgFor,
    MatIconModule,
    MatSliderModule,
    ProductTemplateComponent,
    FormsModule,
    MatPaginatorModule,
    NgIf
  ],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit {
  listOfProducts!: Product[];
  minPrice: number = 1;
  maxPrice: number = 50000;
  listOfCategory!: Category[];
  pageSize: number = 10;
  pageIndex: number = 0;
  paginatedProducts!: Product[];
  pageSizeOptions = [10, 20, 30, 40];
  msgProductNotFound: string = "";
  category: number = 0;

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.getProductsLoaded();
    this.updatePaginatedProducts();
  }

  filter(productName: string) {
    if (this.minPrice !== 1 || this.maxPrice !== 50000) {
      return this.router.navigate(["/products"], { queryParams: { 
        "product-name": productName, "low-price": this.minPrice, "high-price": this.maxPrice
       } });
    }

    if (productName === "") return;

    return this.router.navigate(["/products"], { queryParams: { "product-name": productName } });
  }

  filterCategory() {
    return this.router.navigate(["/products"], { queryParams: { "category": this.category } });
  }

  nextOrPreviousPage(event: any) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updatePaginatedProducts();
  }

  private updatePaginatedProducts() {
    const start = this.pageIndex * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedProducts = this.listOfProducts.slice(start, end);
  }

  private getProductsLoaded(): void {
    this.route.data.subscribe((data) => {
      this.listOfCategory = data["categories"];
      if (data['products'].error) {
        this.msgProductNotFound = data['products'].error;
        this.listOfProducts = [];
        this.updatePaginatedProducts();
      } else {
        this.listOfProducts = data['products'];
        this.updatePaginatedProducts();
      }
    }, (error) => {
      this.msgProductNotFound = error;
      this.listOfProducts = []
    });
  }

}
