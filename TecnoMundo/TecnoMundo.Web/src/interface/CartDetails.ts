import { CartHeader } from "./CartHeader";
import { Product } from "./Product";

export interface CartDetails {
    id?: number,
    cartHeaderId?: number,
    cartHeader?: CartHeader,
    productId: number,
    product: Product,
    count: number
}