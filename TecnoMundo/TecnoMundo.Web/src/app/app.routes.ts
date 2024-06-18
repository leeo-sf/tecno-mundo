import { Routes } from '@angular/router';
import { LoginComponent } from './page/login/login.component';
import { ProductComponent } from './page/product/product.component';
import { authGuard } from './service/_guard/auth.guard';
import { loggedGuard } from './service/_guard/logged.guard';
import { productResolve } from './service/_guard/productResolve';
import { categoryResolve } from './service/_guard/categoryResolve';

export const routes: Routes = [
    {
        path: "login",
        component: LoginComponent,
        canActivate: [loggedGuard]
    },
    {
        path: "products",
        component: ProductComponent,
        resolve: { 
            products : productResolve,
            categories : categoryResolve
        }
    }
];
