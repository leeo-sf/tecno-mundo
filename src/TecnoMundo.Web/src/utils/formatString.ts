import { Injectable } from "@angular/core";

@Injectable({
    providedIn: "root"
})
export class FormatString {
    removePunctuation(cpf: string): string {
        let value = cpf.replace(/\D/g, '');
        return value;
    }

    getServiceName(url: string) {
        const urlSegaments = new URL(url).pathname.split("/");
        const serviceIndex = urlSegaments.findIndex(segment => segment === "v1") + 1;
        return urlSegaments[serviceIndex] || null;
    }
}