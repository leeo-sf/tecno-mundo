import { CartDetails } from "./CartDetails";

export interface OrderMade {
    cartDetails: CartDetails[],
    finalCardPurchased: number,
    totalItems: number,
    purchaseAmount: number,
    discountAmount: number
}