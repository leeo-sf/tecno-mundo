import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../service/auth.service';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [
    MatIconModule,
    RouterLink,
    NgIf,
    CommonModule,
    RouterLinkActive
  ],
  providers: [
    AuthService
  ],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent {
  public isMobile: boolean = false;
  public activeNavLis: string = "flex";
  @ViewChild('navList') navList!: ElementRef;

  constructor(
    private router: Router,
    public authService: AuthService
  ) {}

  logout(): void {
    if (this.authService.logOut()) {
      this.router.navigateByUrl("/");
    }
  }

  public showOrHideNavList(): void {
    let divElement = this.navList.nativeElement;
    
    if (divElement.style.display == 'none') {
      divElement.style.display = "flex";
    }
    else {
      divElement.style.display = "none";
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.checkIfMobile();
  }

  private checkIfMobile() {
    this.isMobile = window.innerWidth <= 999;
    this.isMobile ? this.activeNavLis = "none" : this.activeNavLis = "flex"
  }
}
