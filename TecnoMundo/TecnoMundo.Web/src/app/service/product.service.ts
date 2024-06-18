import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { Product } from '../../interface/Product';
import { Category } from '../../interface/Category';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseApiUrl = environment.baseApiUrlProduct;
  private baseApiUrlProducts = `${this.baseApiUrl}Product`;
  private baseApiUrlGetCategories = `${this.baseApiUrl}Product/categories`;

  constructor(private httpClient: HttpClient) { }

  serviceListProducts(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(this.baseApiUrlProducts);
  }

  serviceListProductCategories(): Observable<Category[]> {
    return this.httpClient.get<Category[]>(this.baseApiUrlGetCategories);
  }

  serviceGetProductById(idProduct: string) {
    return this.httpClient.get<Product>(`${this.baseApiUrlProducts}/${idProduct}`);
  }
}
