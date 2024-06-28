import { Injectable } from "@angular/core";

@Injectable({
    providedIn: "root"
})
export class MethodUtils {
  public defineRole(value: string): string {
    if (value == "" || value === "Client") {
      return "72091";
    }
    else {
      return "19027";
    }
  }
}