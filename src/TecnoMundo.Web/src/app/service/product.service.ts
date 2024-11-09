import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { Product } from '../../interface/Product';
import { Category } from '../../interface/Category';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseApiUrl = environment.baseApiUrlProduct;
  private baseApiUrlGetCategories = `${this.baseApiUrl}categories`;
  private baseApiUrlGetProductsByCategory = `${this.baseApiUrl}by-category`;
  private baseApiUrlGetProductsByName = `${this.baseApiUrl}filter`;

  constructor(private httpClient: HttpClient) { }

  serviceListProducts(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(this.baseApiUrl);
  }

  serviceListProductCategories(): Observable<Category[]> {
    return this.httpClient.get<Category[]>(this.baseApiUrlGetCategories);
  }

  serviceGetProductById(idProduct: string): Observable<Product> {
    return this.httpClient.get<Product>(`${this.baseApiUrl}${idProduct}`);
  }

  serviceGetProductsByCategoryId(idCategory: string): Observable<Product[]> {
    return this.httpClient.get<Product[]>(`${this.baseApiUrlGetProductsByCategory}/${idCategory}`);
  }

  serviceGetProductsByName(productName: string): Observable<Product[]> {
    return this.httpClient.get<Product[]>(`${this.baseApiUrlGetProductsByName}/?name=${productName}`);
  }

  servicePriceRange(productName: string, lowPrice: string, highPrice: string): Observable<Product[]> {
    return this.httpClient.get<Product[]>(`${this.baseApiUrlGetProductsByName}/?name=${productName}&priceOf=${lowPrice}&priceUpTo=${highPrice}`);
  }
}
