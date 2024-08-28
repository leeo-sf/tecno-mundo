import { CommonModule, NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Images } from '../../../interface/Images';
import { CarouselComponent } from '../../template/carousel/carousel.component';
import { MatIconModule } from '@angular/material/icon';
import { Product } from '../../../interface/Product';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CarouselModule } from 'primeng/carousel';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { AddToCartComponent } from '../../template/add-to-cart/add-to-cart.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CarouselComponent,
    MatIconModule,
    CarouselModule,
    ButtonModule,
    TagModule,
    CommonModule,
    RouterLink,
    AddToCartComponent
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
  public newsProduct!: Product[];
  responsiveOptions: any[] | undefined;
  public product: Product[] = [
    { id: 1, name: "Headset Gamer Sem Fio Logitech G533 7.1 Dolby Surround com Drivers de Áudio Pro-G e Bateria Recarregável", price: 1198, imageUrl: "https://m.media-amazon.com/images/I/61CQGpNbraL._AC_SL1000_.jpg", color: "Preto", categoryId: 10, category: { id: 10, name: "Teste" }, description: "testando" }
  ];

  constructor(
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.getNewsProduct();

    this.responsiveOptions = [
        {
            breakpoint: '1199px',
            numVisible: 1,
            numScroll: 1
        },
        {
            breakpoint: '991px',
            numVisible: 2,
            numScroll: 1
        },
        {
            breakpoint: '767px',
            numVisible: 1,
            numScroll: 1
        }
    ];

    this.autoSlide = false;
    this.slideInterval = 4000;
  }

  private getNewsProduct(): void {
    this.route.data.subscribe((data) => {
      this.newsProduct = data["newsProduct"].slice(0, 10);
    });
  }
}
