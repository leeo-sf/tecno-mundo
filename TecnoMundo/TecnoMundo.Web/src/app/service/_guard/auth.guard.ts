import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { AuthService } from '../auth.service';
import { Observable, catchError, map } from 'rxjs';

export const authGuard: CanActivateFn = (): Observable<boolean | UrlTree> => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.isLoggedIn$.pipe(
    map(isLogged => {
      if (!isLogged) {
        router.navigate(['/login']);
        return false;
      }
      else {
        return true;
      }
    }),
    catchError(() => {
      return router.navigate(['/login']);
    })
  );
};