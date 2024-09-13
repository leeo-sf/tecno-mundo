import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderHeader } from '../../interface/OrderHeader';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseApiUrl = environment.baseApiUrlOrder;

  constructor(private httpClient: HttpClient) { }

  serviceGetAllOrders(userId: string, token: string): Observable<OrderHeader[]> {
    const headers = new HttpHeaders({
      "Authorization": `Bearer ${token}`
    });

    return this.httpClient.get<OrderHeader[]>(`${this.baseApiUrl}${userId}`, { headers });
  }
}
