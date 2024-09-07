import { Inject, Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { UserLogin } from '../../interface/UserLogin';
import { tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { UserRegister } from '../../interface/UserRegister';
import { FormatString } from '../../utils/formatString';
import { MethodUtils } from '../../utils/MethodUtils';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseApiUrl: string = environment.baseApiUrlIdentity;
  private baseAuth: string = `${this.baseApiUrl}auth`;
  private baseCreateAccount: string = `${this.baseApiUrl}create-account`;
  private localStorage!: Storage | undefined;

  constructor(
    private httpClient: HttpClient,
    private format: FormatString,
    private utils: MethodUtils,
    @Inject(DOCUMENT) private document: Document
  ) {
    this.localStorage = document.defaultView?.localStorage;
    this.validateToken();
  }

  signIn(user: UserLogin) {
    return this.httpClient.post<any>(this.baseAuth, user).pipe(
      tap((response) => {
        const tokenInfo = this.decodeToken(response.accessToken);
        localStorage.setItem("token", JSON.stringify(response.accessToken));
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

  logOut(): boolean {
    if (this.loggedInUser) {
      this.localStorage?.clear();
      return true;
    }
    return false;
  }

  get loggedInUser(): boolean {
    return !!this.localStorage?.getItem("token");
  }

  private validateToken(): void {
    try {
      const token = localStorage.getItem("token");
      if (this.tokenExpired(token)) {
        //token expired
        localStorage.clear();
        return;
      }
    }
    catch (Error) {
      return;
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
