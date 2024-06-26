import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavBarComponent } from './page/nav-bar/nav-bar.component';
import { FooterComponent } from './page/footer/footer.component';
import { HttpClientModule } from '@angular/common/http';
import { LoadingComponent } from './template/loading/loading.component';

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
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'TecnoMundo.Web';
}
