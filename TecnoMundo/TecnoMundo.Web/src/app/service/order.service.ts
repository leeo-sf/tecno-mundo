import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderHeader } from '../../interface/OrderHeader';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseApiUrl = environment.baseApiUrlOrder;

  constructor(private httpClient: HttpClient) { }

  serviceGetAllOrders(userId: string): Observable<OrderHeader[]> {
    return this.httpClient.get<OrderHeader[]>(`${this.baseApiUrl}${userId}`);
  }
}
