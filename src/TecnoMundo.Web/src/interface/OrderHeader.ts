import { OrderDetail } from "./OrderDetail"

export interface OrderHeader {
    id: number
    userId: string,
    couponCode: string,
    purchaseAmount: string,
    discountAmount: string,
    fistrName: string,
    lastName: string,
    dateTime: Date,
    orderTime: Date,
    phone: string,
    email: string,
    cartTotalItens: string,
    orderDetails: OrderDetail[],
    paymentStatus: boolean
}