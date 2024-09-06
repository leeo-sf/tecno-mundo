import { Injectable } from "@angular/core";

@Injectable({
    providedIn: "root"
})
export class FormatString {
    removePunctuation(cpf: string): string {
        let value = cpf.replace(/\D/g, '');
        return value;
    }
}