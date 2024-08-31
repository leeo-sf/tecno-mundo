import { Component, ElementRef, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../service/auth.service';
import { NgIf } from '@angular/common';
import { response } from 'express';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIf
  ],
  providers: [
    AuthService
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  isLogged!: boolean;

  constructor(
    private authService: AuthService,
    private router: Router,
    private _snackBar: MatSnackBar
  ) {  }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      userName: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(15)
      ]),
      lastName: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(25)
      ]),
      cpf: new FormControl('', [
        Validators.required,
        Validators.minLength(11),
        Validators.maxLength(14)
      ]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.minLength(11),
        Validators.maxLength(15)
      ]),
      userEmail: new FormControl('', [
        Validators.required,
        Validators.email
      ]),
      confirmEmail: new FormControl('', [
        Validators.required
      ]),
      emailConfirmed: new FormControl('', []),
      password: new FormControl('', [
        Validators.required
      ]),
      roleId: new FormControl('', [])
    });
  }

  register(): void {
    if (this.registerForm.invalid) {
      this._snackBar.open("Dados inválidos", "close", { duration: 3 * 1000 });
      return;
    }

    if (!this.emailMatches()) {
      this._snackBar.open("Os e-mail não coindicem", "close", { duration: 3 * 1000 });
      return;
    }

    this.authService.serviceRegister(this.registerForm.value).subscribe((response) => {
      this.router.navigate(["login"]);
    }, (error) => {
      this._snackBar.open(error.error.value, "close", { duration: 3 * 1000 });
    });
  }

  formatCpf(event: FocusEvent): void {
    let value: string = this.cpf;
    if (value) {
      value = value.replace(/\D/g, '');
      if (value.length === 11) {
        value = value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
      }
      this.cpfSet = value;
    }
  }

  formatPhone(event: FocusEvent): void {
    let value: string = this.phone;
    if (value) {
      value = value.replace(/\D/g, ''); 
      if (value.length === 10) {
        value = value.replace(/(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
      } else if (value.length === 11) {
        value = value.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
      }
      this.phone = value;
    }
  }

  preventPaste(event: ClipboardEvent): void {
    event.preventDefault();
  }

  private emailMatches(): boolean {
    if (this.userEmail === this.confirmEmail) {
      return true;
    }
    return false;
  }

  private get cpf(): string {
    return this.registerForm.get('cpf')?.value;
  }

  private get phone(): string {
    return this.registerForm.get('phoneNumber')?.value;
  }

  private get userEmail(): string {
    return this.registerForm.get('userEmail')?.value;
  }

  private get confirmEmail(): string {
    return this.registerForm.get('confirmEmail')?.value;
  }

  private get roleId(): string {
    return this.registerForm.get('roleId')?.value;
  }

  private set cpfSet(value: string) {
    this.registerForm.patchValue({cpf: value});
  }

  private set phone(value: string) {
    this.registerForm.patchValue({phoneNumber: value})
  }
}
