import { ResolveFn } from "@angular/router";
import { Product } from "../../../interface/Product";
import { inject } from "@angular/core";
import { ProductService } from "../product.service";
import { catchError, Observable, of } from "rxjs";

export const productResolve: ResolveFn<Product[] | Product | Observable<any>> = (route, state) => {
    const productService = inject(ProductService);

    if (route.paramMap.get("id")) {
        const idProduct: string = route.paramMap.get("id")!;
        return productService.serviceGetProductById(idProduct);
    }

    if (route.queryParamMap.get("category")) {
        const categoryId = route.queryParamMap.get("category") ?? "";
        return productService.serviceGetProductsByCategoryId(categoryId);
    }

    if (route.queryParamMap.get("product-name") || route.queryParamMap.get("low-price")) {
        const nameProduct: string = route.queryParamMap.get("product-name")!;
        if (route.queryParamMap.get("low-price")) {
            const lowPrice: string = route.queryParamMap.get("low-price")!;
            const highPrice: string = route.queryParamMap.get("high-price")!;
            return productService.servicePriceRange(nameProduct, lowPrice, highPrice).pipe(
                catchError(error => {
                    return of({ products: null, error: nameProduct })
                })
            );
        }
        return productService.serviceGetProductsByName(nameProduct).pipe(
            catchError(error => {
                return of({ products: null, error: nameProduct })
            })
        );
    }

    return productService.serviceListProducts();
}