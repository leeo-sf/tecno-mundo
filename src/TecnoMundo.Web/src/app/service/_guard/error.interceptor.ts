import { HttpErrorResponse, HttpEvent, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { ErrorStateService } from "../error.state.service";
import { FormatString } from "../../../utils/formatString";

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const router = inject(Router);
    const errorStateService = inject(ErrorStateService);
    const utilServiceName = inject(FormatString);
    
    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === 500 || error.status === 0) {
                errorStateService.setErrorState(true);
                errorStateService.setFailedService(utilServiceName.getServiceName(req.url));
                router.navigate(["/error"]);
            }
            return throwError(error);
        })
    )
};