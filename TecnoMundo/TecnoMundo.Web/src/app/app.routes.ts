import { Routes } from '@angular/router';
import { LoginComponent } from './page/login/login.component';
import { authGuard } from './service/_guard/auth.guard';
import { loggedGuard } from './service/_guard/logged.guard';
import { productResolve } from './service/_guard/productResolve';
import { categoryResolve } from './service/_guard/categoryResolve';
import { ProductDetailsComponent } from './page/product-details/product-details.component';
import { RegisterComponent } from './page/register/register.component';
import { ProductComponent } from './page/product/product.component';

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
            products: productResolve,
            categories: categoryResolve
        }
    },
    {
        path: "products/filter/:name",
        component: ProductComponent,
        resolve: {
            products: productResolve,
            categories: categoryResolve
        }
    },
    {
        path: "products/filter/:name/:low-price/:high-price",
        component: ProductComponent,
        resolve: {
            products: productResolve,
            categories: categoryResolve
        }
    },
    {
        path: "products/by-category/filter/:categoryId",
        component: ProductComponent,
        resolve: {
            products: productResolve,
            categories: categoryResolve
        }
    },
    {
        path: "product-details/:id",
        component: ProductDetailsComponent,
        resolve: {
            product: productResolve
        }
    },
    {
        path: "register",
        component: RegisterComponent,
        canActivate: [loggedGuard]
    }
];
