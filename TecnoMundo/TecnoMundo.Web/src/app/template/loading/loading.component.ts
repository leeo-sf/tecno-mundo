import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { LoadingService } from '../../service/loading.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [
    MatIconModule,
    NgIf
  ],
  templateUrl: './loading.component.html',
  styleUrl: './loading.component.css'
})
export class LoadingComponent {

  constructor(public loadingService: LoadingService) {}
}
