import { CommonModule, NgFor } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Images } from '../../../interface/Images';
import { CarouselComponent } from '../../template/carousel/carousel.component';
import { MatIconModule } from '@angular/material/icon';
import { Product } from '../../../interface/Product';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CarouselModule } from 'primeng/carousel';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { AddToCartComponent } from '../../template/add-to-cart/add-to-cart.component';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

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
    AddToCartComponent,
    FormsModule,
    ReactiveFormsModule
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
  public receivePromotions!: FormGroup;
  responsiveOptions: any[] | undefined;
  public product: Product[] = [
    { id: 1, name: "Headset Gamer Sem Fio Logitech G533 7.1 Dolby Surround com Drivers de Áudio Pro-G e Bateria Recarregável", price: 1198, imageUrl: "https://m.media-amazon.com/images/I/61CQGpNbraL._AC_SL1000_.jpg", color: "Preto", categoryId: 10, category: { id: 10, name: "Teste" }, description: "testando" }
  ];

  constructor(
    private route: ActivatedRoute,
    private _snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.getNewsProduct();

    this.receivePromotions = new FormGroup({
      emailToReceive: new FormControl('', [
        Validators.required,
        Validators.email
      ])
    });

    this.autoSlide = false;
    this.slideInterval = 4000;
  }

  private getNewsProduct(): void {
    this.route.data.subscribe((data) => {
      this.newsProduct = data["newsProduct"].slice(0, 10);
    });
  }

  public iWantToReceivePromotions(email: string): void {
    if (this.receivePromotions.invalid) {
      this._snackBar.open("Enter a valid e-mail", "close", { duration: 3 * 1000 });
      return;
    }

    // fazer mensagem de email enviado com sucesso
  }
}
