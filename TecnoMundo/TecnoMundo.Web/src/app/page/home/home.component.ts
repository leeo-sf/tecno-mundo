import { CommonModule, NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Images } from '../../../interface/Images';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    NgFor,
    CommonModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  public images: Images[] = [
    { src: "https://images.pexels.com/photos/3829226/pexels-photo-3829226.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2", alt: "teclado", title: "The best keyboards for your games" },
    { src: "https://images.pexels.com/photos/7848992/pexels-photo-7848992.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2", alt: "headser-gamer", title: "Listen as if you were in the game" },
    { src: "https://images.pexels.com/photos/7238759/pexels-photo-7238759.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2", alt: "setup", title: "Build your corner with us" }
  ];
  public selectedIndex: number = 0;
  public controls: boolean = true;
  public autoSlide: boolean = true;
  public slideInterval: number = 4000;

  ngOnInit(): void {
    if (this.autoSlide) {
      this.autoSlideImages();
    }
  }

  private autoSlideImages(): void {
    setInterval(() => {
      this.onNextClick();
    }, this.slideInterval);
  }

  public selectedImage(index: number): void {
    this.selectedIndex = index;
  }

  public onPrevClick(): void {
    if (this.selectedIndex === 0) {
      this.selectedIndex = this.images.length - 1;
    }
    else {
      this.selectedIndex--;
    }
  }

  public onNextClick(): void {
    if (this.selectedIndex == this.images.length - 1) {
      this.selectedIndex = 0;
    }
    else {
      this.selectedIndex++;
    }
  }
}
