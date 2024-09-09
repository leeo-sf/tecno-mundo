import { APP_INITIALIZER, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { LoadingComponent } from './template/loading/loading.component';
import { NavBarComponent } from './template/nav-bar/nav-bar.component';
import { FooterComponent } from './template/footer/footer.component';
import { environment } from '../environment/environment';

export function loadConfig(http: HttpClient) {
  return () => http.get('assets/config.json')
    .toPromise()
    .then((config: any) => {
      Object.assign(environment, config);
    });
}

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
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: loadConfig,
      deps: [HttpClient],
      multi: true
    }
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'TecnoMundo.Web';
}
