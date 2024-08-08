import { CartDetails } from "./CartDetails";

export interface Checkout {
    id?: number,
    messageCreated: string,
    userId: string,
    couponCode: string,
    purchaseAmount: number,
    discountAmount: number,
    fistrName: string,
    lastName: string,
    dateTime: string,
    phone: string,
    email: string,
    cardNumber: string,
    cvv: string,
    expireMonthYear: string,
    cartTotalItems: number,
    cartDetails: CartDetails[]
}