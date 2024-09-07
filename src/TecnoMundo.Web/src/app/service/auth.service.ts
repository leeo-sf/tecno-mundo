import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { UserLogin } from '../../interface/UserLogin';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { UserRegister } from '../../interface/UserRegister';
import { FormatString } from '../../utils/formatString';
import { MethodUtils } from '../../utils/MethodUtils';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseApiUrl: string = environment.baseApiUrlIdentity;
  private baseAuth: string = `${this.baseApiUrl}auth`;
  private baseCreateAccount: string = `${this.baseApiUrl}create-account`;
  userName: string = "";
  private isLoggedIn = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.isLoggedIn.asObservable();

  constructor(
    private httpClient: HttpClient,
    private format: FormatString,
    private utils: MethodUtils
  ) {
    this.isLoggedIn.next(false);
    this.validateToken();
  }

  signIn(user: UserLogin) {
    return this.httpClient.post<any>(this.baseAuth, user).pipe(
      map((response) => {
        const tokenInfo = this.decodeToken(response.accessToken);
        const responseLogin = { 
          accessToken: JSON.stringify(response.accessToken), 
          "user_name": JSON.stringify(tokenInfo.unique_name), 
          "user_id": JSON.stringify(tokenInfo.UserId) 
        };
        this.isLoggedIn.next(true);
        console.log(this.isLoggedIn.getValue());
        return responseLogin;
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
    this.isLoggedIn.next(false);
  }

  get loggedInUser(): boolean {
    return this.isLoggedIn.value;
  }

  private validateToken(): void {
    try {
      const token = localStorage.getItem("token");
      if (this.tokenExpired(token)) {
        //token expired
        localStorage.clear();
        return;
      }
      
      const tokenInfo = this.decodeToken(String(token));
      this.userName = tokenInfo.unique_name;
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
