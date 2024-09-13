import { Component, Inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { LoadingComponent } from './template/loading/loading.component';
import { NavBarComponent } from './template/nav-bar/nav-bar.component';
import { FooterComponent } from './template/footer/footer.component';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    NavBarComponent,
    FooterComponent,
    HttpClientModule,
    LoadingComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [ 
    { provide: 'BASE_API_URL_PRODUCT', useValue: environment.baseApiUrlProduct },
    { provide: 'BASE_API_URL_IDENTITY', useValue: environment.baseApiUrlIdentity },
    { provide: 'BASE_API_URL_CART', useValue: environment.baseApiUrlCart },
    { provide: 'BASE_API_URL_COUPON', useValue: environment.baseApiUrlCoupon },
    { provide: 'BASE_API_URL_ORDER', useValue: environment.baseApiUrlOrder }
  ]
})
export class AppComponent {
  title = 'TecnoMundo.Web';
}
