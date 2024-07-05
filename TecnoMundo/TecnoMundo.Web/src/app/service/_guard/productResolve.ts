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

    if (route.paramMap.get("categoryId")) {
        const categoryId: number = Number.parseInt(route.paramMap.get("categoryId")!);
        return productService.serviceGetProductsByCategoryId(categoryId);
    }

    if (route.paramMap.get("name") || route.paramMap.get("low-price")) {
        const nameProduct: string = route.paramMap.get("name")!;
        if (route.paramMap.get("low-price")) {
            const lowPrice: string = route.paramMap.get("low-price")!;
            const highPrice: string = route.paramMap.get("high-price")!;
            return productService.servicePriceRange(nameProduct, lowPrice, highPrice).pipe(
                catchError(error => {
                    console.log("Erro no resvolve ", error);
                    return of({ products: null, error: nameProduct })
                })
            );
        }
        return productService.serviceGetProductsByName(nameProduct).pipe(
            catchError(error => {
                console.log("Erro no resvolve ", error);
                return of({ products: null, error: nameProduct })
            })
        );
    }

    return productService.serviceListProducts();
}