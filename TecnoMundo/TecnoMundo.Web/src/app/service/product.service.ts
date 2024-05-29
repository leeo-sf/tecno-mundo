import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../../interface/Product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseApiUrl = environment.baseApiUrlProduct;
  private baseApiUrlGetAllProducts = `${this.baseApiUrl}Product`;

  constructor(private httpClient: HttpClient) { }

  serviceListProducts(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(this.baseApiUrlGetAllProducts);
  }
}
