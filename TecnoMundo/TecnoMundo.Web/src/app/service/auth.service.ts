import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { UserLogin } from '../../interface/UserLogin';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseApiUrl: string = environment.baseApiUrlIdentity;
  private baseAuth: string = `${this.baseApiUrl}auth`

  constructor(private httpClient: HttpClient) { }

  signIn(user: UserLogin) {
    return this.httpClient.post(this.baseAuth, user).pipe(
      tap((response: any) => {
        console.log(response);
      })
    )
  }
}
