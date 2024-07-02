import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { LoadingComponent } from './template/loading/loading.component';
import { NavBarComponent } from './template/nav-bar/nav-bar.component';
import { FooterComponent } from './template/footer/footer.component';

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
