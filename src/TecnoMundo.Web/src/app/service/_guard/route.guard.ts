import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const routeGuard: CanActivateFn = (route, state): boolean => {
  const router = inject(Router);
  const previousUrl = router.getCurrentNavigation()?.previousNavigation?.finalUrl?.toString();
  const navigation = router.getCurrentNavigation();
  const allowedRouteCart = "/my-cart";
  const allowedRouteFinalizeOrder = "/finalize-order";
  

  if (previousUrl === allowedRouteCart && navigation?.extras.state?.['finalizeOrder']) {
    return true;
  }

  if (previousUrl === allowedRouteFinalizeOrder && navigation?.extras.state?.['orderMade']) {
    return true;
  }

  return false;
};
