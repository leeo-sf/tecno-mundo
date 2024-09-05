import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { ProductTemplateComponent } from '../../template/product-template/product-template.component';
import { MatSliderModule } from '@angular/material/slider'
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from '../../../interface/Category';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { MatPaginatorModule } from '@angular/material/paginator';
import { Product } from '../../../interface/Product';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';

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
    NgIf,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    CommonModule
  ],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit {
  listOfProducts!: Product[];
  minPrice: number = 1;
  maxPrice: number = 20000;
  listOfCategory!: Category[];
  pageSize: number = 10;
  pageIndex: number = 0;
  paginatedProducts!: Product[];
  pageSizeOptions = [10, 20, 30, 40];
  msgProductNotFound: string = "";
  category: number = 0;
  isMobile: boolean = false;
  showOrHideFilters$: string = 'flex';
  @ViewChild('filters') hideOrShowFilters!: ElementRef;
  @ViewChild('btnFilters') btnFilters!: ElementRef;

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.checkIfMobile();
  }

  ngOnInit(): void {
    this.getProductsLoaded();
    this.updatePaginatedProducts();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.checkIfMobile();
  }

  private checkIfMobile() {
    this.isMobile = window.innerWidth <= 768;
    this.isMobile ? this.showOrHideFilters$ = "none" : this.showOrHideFilters$ = "flex"
  }

  public showOrHideFilters(): void {
    let divElement = this.hideOrShowFilters.nativeElement;
    let divElementBtn = this.btnFilters.nativeElement;
    
    if (divElement.style.display == 'none') {
      divElement.style.display = "flex";
      divElementBtn.style.transform = "rotate(90deg)";
    }
    else {
      divElement.style.display = "none";
      divElementBtn.style.transform = "rotate(270deg)";
    }
  }

  filter(productName: string) {
    if (this.minPrice !== 1 || this.maxPrice !== 20000) {
      this.cleanFilter("category", false);
      return this.router.navigate(["/products"], { queryParams: { 
        "product-name": productName, "low-price": this.minPrice, "high-price": this.maxPrice
       } });
    }

    if (productName === "") return;

    return this.router.navigate(["/products"], { queryParams: { "product-name": productName } });
  }

  filterCategory() {
    this.cleanFilter("price", false);
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

  public cleanFilter(tag: string, refresh?: boolean) {
    if (tag.includes("category")) {
      return this.cleanCategory();
    }
    
    if (tag.includes("price")) {
      return this.cleanPrice();
    }

    this.cleanAll();
    refresh ? this.router.navigate(["/products"]) : false
  }

  private cleanCategory(): void {
    this.category = 0;
  }

  private cleanPrice(): void {
    this.minPrice = 1;
    this.maxPrice = 20000;
  }

  private cleanAll(): void {
    this.cleanCategory();
    this.cleanPrice();
  }

}
