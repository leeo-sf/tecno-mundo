import { CommonModule, NgFor } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Images } from '../../../interface/Images';

@Component({
  selector: 'app-carousel',
  standalone: true,
  imports: [
    NgFor,
    CommonModule
  ],
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.css'
})
export class CarouselComponent {
  @Input() images!: Images[];
  public selectedIndex: number = 0;
  @Input() autoSlide!: boolean;
  @Input() slideInterval!: number;

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
