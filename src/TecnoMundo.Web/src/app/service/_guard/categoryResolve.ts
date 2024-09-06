import { ResolveFn } from "@angular/router";
import { Product } from "../../../interface/Product";
import { inject } from "@angular/core";
import { ProductService } from "../product.service";
import { Category } from "../../../interface/Category";

export const categoryResolve: ResolveFn<Category[]> = (route, state) => {
    const productService = inject(ProductService);
    return productService.serviceListProductCategories();
}