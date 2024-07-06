import { CartDetails } from "./CartDetails";
import { CartHeader } from "./CartHeader";

export interface Cart {
    cartHeader: CartHeader
    cartDetails: CartDetails[]
}