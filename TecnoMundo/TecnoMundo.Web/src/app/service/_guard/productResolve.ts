import { ResolveFn } from "@angular/router";
import { Product } from "../../../interface/Product";
import { inject } from "@angular/core";
import { ProductService } from "../product.service";

export const productResolve: ResolveFn<Product[]> = (route, state) => {
    const productService = inject(ProductService);
    return productService.serviceListProducts();
}