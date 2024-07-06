import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cart } from '../../interface/Cart';
import { CartHeader } from '../../interface/CartHeader';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private baseApiUrl = environment.baseApiUrlCart;
  private baseApiUrlFindCart = `${this.baseApiUrl}find-cart`;
  private baseApiUrlAddCart = `${this.baseApiUrl}add-cart`;
  private baseApiUrlUpdateCart = `${this.baseApiUrl}update-cart`;
  private baseApiUrlRemoveCart = `${this.baseApiUrl}remove-cart`;
  private baseApiUrlCheckout = `${this.baseApiUrl}checkout`;

  constructor(
    private httpClient: HttpClient
  ) { }

  serviceFindCartByUserId(userId: string, token: string): Observable<Cart> {
    const headers = new HttpHeaders({
      "Authorization": `Bearer ${token}`
    });

    return this.httpClient.get<Cart>(`${this.baseApiUrlFindCart}/${userId}`, { headers });
  }

  serviceAddItemToCart(cart: Cart, token: string): Observable<Cart> {
    const header = new HttpHeaders({
      "Authorization": `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.post<Cart>(this.baseApiUrlAddCart, JSON.stringify(cart), { headers: header });
  }

  serviceUpdateToCart(cart: Cart, token: string): Observable<Cart> {
    const headers = new HttpHeaders({
      "Authorization": `Bearer ${token}`
    });

    return this.httpClient.put<Cart>(this.baseApiUrlUpdateCart, JSON.stringify(cart), { headers });
  }

  serviceRemoveFromCart(cartId: number, token: string): Observable<boolean> {
    const headers = new HttpHeaders({
      "Authorization": `Bearer ${token}`
    });

    return this.httpClient.delete<boolean>(`${this.baseApiUrlRemoveCart}/${cartId}`, { headers });
  }

  serviceClearCart(userId: string, token: string) {

  }

  serviceCheckout(cartHeader: CartHeader, token: string) {

  }

  //serviceApplyCoupon(cart: Cart, couponCode: string, token: string) {}

  //serviceRemoveCoupon(userId: string, token: string) {}
}
