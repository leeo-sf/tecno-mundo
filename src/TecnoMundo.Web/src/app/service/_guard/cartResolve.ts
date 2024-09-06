import { ResolveFn } from "@angular/router";
import { Cart } from "../../../interface/Cart";
import { inject } from "@angular/core";
import { CartService } from "../cart.service";

export const cartResolve: ResolveFn<Cart> = (route, state) => {
    const cartService = inject(CartService);
    const token: string = localStorage.getItem("token") ?? "";
    const userId: string = localStorage.getItem("user-id") ?? "";

    return cartService.serviceFindCartByUserId(JSON.parse(userId), JSON.parse(token));
}