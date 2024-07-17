import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private loading: boolean = false;

  constructor() { }

  get isLoading(): boolean {
    return this.loading;
  }

  show() {
    this.loading = true;
  }

  hide() {
    this.loading = false;
  }
}
