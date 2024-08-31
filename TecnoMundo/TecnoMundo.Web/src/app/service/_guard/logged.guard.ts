import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { AuthService } from '../auth.service';
import { Observable, catchError, map } from 'rxjs';

export const loggedGuard: CanActivateFn = (): Observable<boolean | UrlTree> | boolean => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.loggedInUser) {
    return false;
  }
  else {
    return true;
  }
};
