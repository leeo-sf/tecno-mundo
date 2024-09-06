import { Cart } from "./Cart";
import { Coupon } from "./Coupon";

export interface FinalizeOrder {
    cart: Cart,
    coupon?: Coupon
}