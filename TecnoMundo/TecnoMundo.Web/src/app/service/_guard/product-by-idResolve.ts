import { ResolveFn, Router } from "@angular/router";
import { Product } from "../../../interface/Product";
import { inject } from "@angular/core";
import { ProductService } from "../product.service";

export const productByIdResolve: ResolveFn<Product | null> = (route, state) => {
    const productService = inject(ProductService);
    const idProduct = route.paramMap.get('id');
    if (idProduct === null) {
        return null;
    }

    return productService.serviceGetProductById(idProduct);
}