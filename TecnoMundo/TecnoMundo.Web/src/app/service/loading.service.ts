import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  public loading: boolean = false;

  constructor() { }

  loadingTrue() {
    this.loading = true;
  }

  loadingFalse() {
    this.loading = false;
  }
}
