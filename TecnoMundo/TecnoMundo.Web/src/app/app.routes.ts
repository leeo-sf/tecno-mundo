import { Routes } from '@angular/router';
import { LoginComponent } from './page/login/login.component';
import { authGuard } from './service/_guard/auth.guard';
import { loggedGuard } from './service/_guard/logged.guard';
import { productResolve } from './service/_guard/productResolve';
import { categoryResolve } from './service/_guard/categoryResolve';
import { ProductDetailsComponent } from './page/product-details/product-details.component';
import { RegisterComponent } from './page/register/register.component';
import { ProductComponent } from './page/product/product.component';
import { CartComponent } from './page/cart/cart.component';
import { cartResolve } from './service/_guard/cartResolve';
import { FinalizeOrderComponent } from './page/finalize-order/finalize-order.component';
import { PurchaseMadeComponent } from './template/purchase-made/purchase-made.component';
import { routeGuard } from './service/_guard/route.guard';
import { OrderComponent } from './page/order/order.component';
import { orderResolve } from './service/_guard/orderResolve';
import { HomeComponent } from './page/home/home.component';

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
        },
        runGuardsAndResolvers: 'paramsOrQueryParamsChange'
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
    },
    {
        path: "my-cart",
        component: CartComponent,
        runGuardsAndResolvers: 'always',
        canActivate: [ authGuard ],
        resolve: { cart: cartResolve },
    },
    {
        path: "finalize-order",
        component: FinalizeOrderComponent,
        canActivate: [authGuard, routeGuard]
    },
    {
        path: "finalize-order/order",
        component: PurchaseMadeComponent,
        canActivate: [authGuard, routeGuard]
    },
    {
        path: "orders",
        component: OrderComponent,
        canActivate: [ authGuard ],
        resolve: { orders: orderResolve }
    },
    {
        path: "",
        component: HomeComponent
    },
    {
        path: "**",
        redirectTo: "",
        pathMatch: "full"
    }
];
