export interface CartHeader {
    id?: number,
    userId: string,
    couponCode?: string,
    //valor final da compra
    purchaseAmount?: number
}