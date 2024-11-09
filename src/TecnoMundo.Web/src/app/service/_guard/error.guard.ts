import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { ErrorStateService } from "../error.state.service";

export const errorGuard: CanActivateFn = (): boolean => {
    const router = inject(Router);
    const errorStateService = inject(ErrorStateService);

    if (errorStateService.getErrorState()) {
        errorStateService.setErrorState(false);
        return true;
    }
    else {
        router.navigate(["/"]);
        return false;
    }
  };