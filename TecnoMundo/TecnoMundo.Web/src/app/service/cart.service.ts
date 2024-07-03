import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cart } from '../../interface/Cart';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private baseApiUrl = environment.baseApiUrlCart;
  private baseApiUrlAddCart = `${this.baseApiUrl}add-cart`;

  constructor(
    private httpClient: HttpClient
  ) { }

  serviceAddCart(cart: Cart): Observable<Cart> {
    return this.httpClient.post<Cart>(this.baseApiUrlAddCart, cart);
  }
}
