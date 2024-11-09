import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ErrorStateService } from '../../service/error.state.service';
import { NgFor } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-error-page',
  standalone: true,
  imports: [
    NgFor,
    MatIconModule
  ],
  providers: [
    NgFor
  ],
  templateUrl: './error-page.component.html',
  styleUrl: './error-page.component.css'
})
export class ErrorPageComponent implements OnInit {
  public failedService: string | null = ""
  public listOfPossibleErrors = [
    {error: "Servers restarted"},
    {error: "API`s turned off"},
    {error: "Our service may be undergoing maintenance"}
  ]

  constructor(private errorStateService: ErrorStateService) {}

  ngOnInit(): void {
    this.errorStateService.resetErrorState();
    this.errorStateService.getNameFailedService().subscribe((serviceName) => {
      this.failedService = serviceName
    });
  }
}
