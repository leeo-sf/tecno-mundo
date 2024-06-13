import { Routes } from '@angular/router';
import { LoginComponent } from './page/login/login.component';
import { ProductComponent } from './page/product/product.component';
import { authGuard } from './service/_guard/auth.guard';
import { loggedGuard } from './service/_guard/logged.guard';

export const routes: Routes = [
    {
        path: "",
        pathMatch: "full",
        redirectTo: "home"
    },
    {
        path: "**",
        redirectTo: "home"
    },
    {
        path: "login",
        component: LoginComponent,
        canActivate: [loggedGuard]
    },
    {
        path: "products",
        component: ProductComponent,
        //pathMatch: 'full',
        canActivate: [authGuard]
    }
];
