import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  public loading: boolean = false;

  constructor() { }

  show() {
    this.loading = true;
  }

  hide() {
    this.loading = false;
  }
}
