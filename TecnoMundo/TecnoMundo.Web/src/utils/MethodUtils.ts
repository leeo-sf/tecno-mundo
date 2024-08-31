import { Injectable } from "@angular/core";

@Injectable({
    providedIn: "root"
})
export class MethodUtils {
  public defineRole(value: string): string {
    if (value == "" || value === "Client") {
      return "389164";
    }
    else {
      return "107927";
    }
  }
}