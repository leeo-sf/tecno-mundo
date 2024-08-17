import { ResolveFn } from "@angular/router";
import { OrderHeader } from "../../../interface/OrderHeader";
import { inject } from "@angular/core";
import { OrderService } from "../order.service";

export const orderResolve: ResolveFn<OrderHeader[] | null> = (route, state) => {
    const orderService = inject(OrderService);
    const userId: string = localStorage.getItem("user-id") ?? "";

    return orderService.serviceGetAllOrders(userId);
}