import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Coupon } from '../../interface/Coupon';

@Injectable({
  providedIn: 'root'
})
export class CouponService {
  private baseApiUrl: string = environment.baseApiUrlCoupon;

  constructor(private httpClient: HttpClient) { }

  serviceGetCouponCode(couponCode: string, token: string): Observable<Coupon> {
    const header = new HttpHeaders({
      "Authorization": `Bearer ${token}`,
    });

    return this.httpClient.get<Coupon>(`${this.baseApiUrl}${couponCode}`, { headers: header });
  }
}
