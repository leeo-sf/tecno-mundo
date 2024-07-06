import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { ProductTemplateComponent } from '../../template/product-template/product-template.component';
import { MatSliderModule } from '@angular/material/slider'
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from '../../../interface/Category';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [
    NgFor,
    MatIconModule,
    MatSliderModule,
    ProductTemplateComponent,
    FormsModule
  ],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit {
  minPrice: number = 1;
  maxPrice: number = 50000;
  listOfCategory!: Category[];

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.listOfCategory = data["categories"];
    });
  }

  filter(productName: string) {
    if (this.minPrice !== 1 || this.maxPrice !== 50000) {
      return this.router.navigate(["/products/filter"], { queryParams: { 
        "product-name": productName, "low-price": this.minPrice, "high-price": this.maxPrice
       } });
    }

    if (productName === "") return;

    return this.router.navigate(["/products/filter"], { queryParams: { "product-name": productName } });
  }

  filterCategory(categoryId: number) {
    return this.router.navigate(["/products/filter/by-category/", categoryId]);
  }
}
