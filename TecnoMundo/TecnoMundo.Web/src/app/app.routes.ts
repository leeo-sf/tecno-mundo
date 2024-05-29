import { Routes } from '@angular/router';
import { LoginComponent } from './template/login/login.component';
import { ProductComponent } from './page/product/product.component';

export const routes: Routes = [
    {
        path: "login",
        component: LoginComponent
    },
    {
        path: "products",
        component: ProductComponent
    }
];
