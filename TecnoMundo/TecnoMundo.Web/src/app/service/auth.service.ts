import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { UserLogin } from '../../interface/UserLogin';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { UserRegister } from '../../interface/UserRegister';
import { FormatString } from '../../utils/formatString';
import { MethodUtils } from '../../utils/MethodUtils';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _isLoggedIn$ = new BehaviorSubject<boolean>(false);
  private baseApiUrl: string = environment.baseApiUrlIdentity;
  private baseAuth: string = `${this.baseApiUrl}auth`;
  private baseCreateAccount: string = `${this.baseApiUrl}create-account`;
  isLoggedIn$ = this._isLoggedIn$.asObservable();
  userName: string = "";

  constructor(
    private httpClient: HttpClient,
    private router: Router,
    private format: FormatString,
    private utils: MethodUtils
  ) {
    const token = this.validateToken();
    this._isLoggedIn$.next(!!token);
  }

  signIn(user: UserLogin) {
    return this.httpClient.post<any>(this.baseAuth, user).pipe(
      tap((response) => {
        this._isLoggedIn$.next(true);
        const tokenInfo = this.decodeToken(response.value.acessToken);
        localStorage.setItem("token", JSON.stringify(response.value.acessToken));
        localStorage.setItem("user-name", JSON.stringify(tokenInfo.unique_name));
        localStorage.setItem("user-id", JSON.stringify(tokenInfo.UserId));
      })
    );
  }

  serviceRegister(user: UserRegister) {
    user.cpf = this.format.removePunctuation(user.cpf);
    user.phoneNumber = this.format.removePunctuation(user.phoneNumber);
    user.emailConfirmed = true;
    user.roleId = this.utils.defineRole(user.roleId);

    return this.httpClient.post(this.baseCreateAccount, user);
  }

  logOut() {
    if (this._isLoggedIn$.getValue()) {
      localStorage.clear();
      this.router.navigate(['/']);
    }
  }

  private validateToken(): any {
    try {
      const token = localStorage.getItem("token");
      if (this.tokenExpired(token)) {
        //token expired
        localStorage.removeItem("token");
        localStorage.removeItem("user-name");
        localStorage.removeItem("user-id");
        this._isLoggedIn$.next(false);
        return;
      }
      
      const tokenInfo = this.decodeToken(String(token));
      this._isLoggedIn$.next(!!token);
      this.userName = tokenInfo.unique_name;
      return token;
    }
    catch (Error) {
      this._isLoggedIn$.next(false);
    }
  }

  private tokenExpired(token: any): boolean {
    const expiry = (JSON.parse(atob(token.split('.')[1]))).exp;
    return (Math.floor((new Date).getTime() / 1000)) >= expiry;
  }

  private decodeToken(token: string): any {
    try {
      return jwtDecode(token);
    }
    catch (Error) {
      return null
    }
  }
}
