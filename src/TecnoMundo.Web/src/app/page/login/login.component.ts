import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../service/auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoadingService } from '../../service/loading.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    AuthService
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private authService: AuthService, 
    private router: Router,
    private _snackBar: MatSnackBar,
    private loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      userEmail: new FormControl('', [
        Validators.required,
        Validators.email
      ]),
      password: new FormControl('', [
        Validators.required
      ])
    })
  };

  login(): void {
    if (this.loginForm.invalid) {
      this._snackBar.open("Invalid login data", "close", { duration: 3 * 1000 });
      return
    }

    this.loadingService.show();
    this.authService.signIn(this.loginForm.value).subscribe((response) => {
      localStorage.setItem("token", response.accessToken);
      localStorage.setItem("user-name", response.user_name);
      localStorage.setItem("user-id", response.user_id);
      this.loadingService.hide();
      this.router.navigate(['']);
    }, (error) => {
      this.loadingService.hide();
      if (error.error.value) {
        this._snackBar.open(error.error.value, "close", { duration: 3 * 1000 });
      }
      else {
        this._snackBar.open(error.error.errors.Password[0], "close", { duration: 3 * 1000 });
      }
    });
  }
}
