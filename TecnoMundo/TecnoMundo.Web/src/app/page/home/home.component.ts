import { CommonModule, NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Images } from '../../../interface/Images';
import { CarouselComponent } from '../../template/carousel/carousel.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CarouselComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  public images: Images[] = [
    { src: "https://images.pexels.com/photos/3829226/pexels-photo-3829226.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2", alt: "teclado", title: "The best keyboards for your games", nameProduct: "teclado" },
    { src: "https://images.pexels.com/photos/7848992/pexels-photo-7848992.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2", alt: "headser-gamer", title: "Listen as if you were in the game", nameProduct: "headset" },
    { src: "https://images.pexels.com/photos/7238759/pexels-photo-7238759.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2", alt: "setup", title: "Build your corner with us", nameProduct: "" }
  ];
  public autoSlide!: boolean;
  public slideInterval!: number;

  ngOnInit(): void {
    this.autoSlide = true;
    this.slideInterval = 4000;
  }
}
