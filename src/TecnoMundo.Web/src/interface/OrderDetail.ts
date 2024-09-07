import { Product } from "./Product";

export interface OrderDetail {
    orderHeaderId: number,
    productId: number,
    product: Product,
    count: number,
}