import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorStateService {
  private failedServiceSubject = new BehaviorSubject<string | null>(null);
  private hasError: boolean = false;

  setErrorState(state: boolean) {
    this.hasError = state;
  }

  getErrorState(): boolean {
    return this.hasError;
  }

  resetErrorState() {
    this.hasError = false;
  }

  setFailedService(serviceName: string | null) {
    this.failedServiceSubject.next(serviceName)
  }

  getNameFailedService(): Observable<string | null> {
    return this.failedServiceSubject.asObservable();
  }
}